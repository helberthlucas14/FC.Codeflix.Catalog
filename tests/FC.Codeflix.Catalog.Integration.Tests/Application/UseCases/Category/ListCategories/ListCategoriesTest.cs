using FluentAssertions;
using FC.Codeflix.Catalog.Infra.Data.EF.Repositories;
using FC.Codeflix.Catalog.Application.UseCases.Category.ListCategories;
using UseCase = FC.Codeflix.Catalog.Application.UseCases.Category.ListCategories;
using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using FC.Codeflix.Catalog.Domain.SeedWork;
using FC.Codeflix.Catalog.Infra.Data.EF;

namespace FC.Codeflix.Catalog.Integration.Tests.Application.UseCases.Category.ListCategories
{
    [Collection(nameof(ListCategoriesTestFixture))]
    public class ListCategoriesTest
    {
        private readonly ListCategoriesTestFixture _fixture;

        public ListCategoriesTest(ListCategoriesTestFixture fixture) => _fixture = fixture;


        [Fact(DisplayName = nameof(SearchReturnListAndTotal))]
        [Trait("Integration/Application", "ListCategories -  Use Cases")]
        public async Task SearchReturnListAndTotal()
        {
            CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();

            var exampleCategoriesList = _fixture.GetExampleCategoriesList(10);
            await dbContext.AddRangeAsync(exampleCategoriesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var repository = new CategoryRepository(dbContext);

            var searchInput = new ListCategoriesInput(1, 20);

            var usecase = new UseCase.ListCategories(repository);

            var output = await usecase.Handle(searchInput, CancellationToken.None);

            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.PerPage.Should().Be(searchInput.PerPage);
            output.Total.Should().Be(exampleCategoriesList.Count);
            output.Items.Should().HaveCount(exampleCategoriesList.Count);

            foreach (CategoryModelOutput outputItem in output.Items)
            {
                var exampleItem = exampleCategoriesList.Find(
                    category => category.Id == outputItem.Id
                );

                exampleItem.Should().NotBeNull();
                outputItem.Id.Should().Be(exampleItem.Id);
                outputItem.Name.Should().Be(exampleItem.Name);
                outputItem.Description.Should().Be(exampleItem.Description);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
            }
        }

        [Fact(DisplayName = nameof(SearchReturnsEmptyWhenPersistenceIsEmpty))]
        [Trait("Integration/Application", "ListCategories -   Use Cases")]
        public async Task SearchReturnsEmptyWhenPersistenceIsEmpty()
        {
            CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();

            var repository = new CategoryRepository(dbContext);
            var searchInput = new ListCategoriesInput(1, 20, "", "", SearchOrder.Asc);

            var usecase = new UseCase.ListCategories(repository);
            var output = await usecase.Handle(searchInput, CancellationToken.None);

            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.PerPage.Should().Be(searchInput.PerPage);
            output.Total.Should().Be(0);
            output.Items.Should().HaveCount(0);
        }


        [Theory(DisplayName = nameof(SearchReturnsEmptyPaginated))]
        [Trait("Integration/Application", "ListCategories -   Use Cases")]
        [InlineData(10, 1, 5, 5)]
        [InlineData(10, 2, 5, 5)]
        [InlineData(7, 2, 5, 2)]
        [InlineData(7, 3, 5, 0)]
        public async Task SearchReturnsEmptyPaginated(
            int quantityCategoryToGenerate,
            int page,
            int perPage,
            int expectedQuantityItems
            )
        {
            CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();

            var exampleCategoriesList = _fixture.GetExampleCategoriesList(quantityCategoryToGenerate);
            await dbContext.AddRangeAsync(exampleCategoriesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var repository = new CategoryRepository(dbContext);

            var searchInput = new ListCategoriesInput(page, perPage, "", "", SearchOrder.Asc);

            var usecase = new UseCase.ListCategories(repository);
            var output = await usecase.Handle(searchInput, CancellationToken.None);

            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.PerPage.Should().Be(searchInput.PerPage);
            output.Total.Should().Be(exampleCategoriesList.Count);
            output.Items.Should().HaveCount(expectedQuantityItems);

            foreach (CategoryModelOutput outputItem in output.Items)
            {
                var exampleItem = exampleCategoriesList.Find(
                    category => category.Id == outputItem.Id
                );

                exampleItem.Should().NotBeNull();
                outputItem.Id.Should().Be(exampleItem.Id);
                outputItem.Name.Should().Be(exampleItem.Name);
                outputItem.Description.Should().Be(exampleItem.Description);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
            }
        }

        [Theory(DisplayName = nameof(SearchByText))]
        [Trait("Integration/Application", "ListCategories -  Use Cases")]
        [InlineData("Action", 1, 5, 1, 1)]
        [InlineData("Horror", 1, 5, 3, 3)]
        [InlineData("Horror", 2, 5, 0, 3)]
        [InlineData("Sci-fi", 1, 5, 4, 4)]
        [InlineData("Sci-fi", 1, 2, 2, 4)]
        [InlineData("Sci-fi", 2, 3, 1, 4)]
        [InlineData("Sci-fi Other", 1, 3, 0, 0)]
        [InlineData("Robots", 1, 5, 2, 2)]
        public async Task SearchByText(
              string search,
              int page,
              int perPage,
              int expectedQuantityItemsReturned,
              int expextedQuantityTotalItems
    )
        {
            CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();

            var exampleCategoriesList =
                _fixture.GetExampleCategoriesListWithNames(new List<string>() {
                "Action",
                "Horror",
                "Horror - Robots",
                "Horror - Bases on  Real Facts",
                "Drama",
                "Sci-fi IA",
                "Sci-fi Space",
                "Sci-fi Robots",
                "Sci-fi Future",
            });

            await dbContext.AddRangeAsync(exampleCategoriesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var repository = new CategoryRepository(dbContext);

            var searchInput = new ListCategoriesInput(page, perPage, search, "", SearchOrder.Asc);

            var usecase = new UseCase.ListCategories(repository);
            var output = await usecase.Handle(searchInput, CancellationToken.None);

            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.PerPage.Should().Be(searchInput.PerPage);
            output.Total.Should().Be(expextedQuantityTotalItems);
            output.Items.Should().HaveCount(expectedQuantityItemsReturned);

            foreach (CategoryModelOutput outputItem in output.Items)
            {
                var exampleItem = exampleCategoriesList.Find(
                    category => category.Id == outputItem.Id
                );

                exampleItem.Should().NotBeNull();
                outputItem.Id.Should().Be(exampleItem.Id);
                outputItem.Name.Should().Be(exampleItem.Name);
                outputItem.Description.Should().Be(exampleItem.Description);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
            }
        }

        [Theory(DisplayName = nameof(SearchOrdered))]
        [Trait("Integration/Infra.Data", "CategoryRepository -  Repositories")]
        [InlineData("name", "asc")]
        [InlineData("name", "desc")]
        [InlineData("id", "asc")]
        [InlineData("id", "desc")]
        [InlineData("createdAt", "asc")]
        [InlineData("createdAt", "desc")]
        [InlineData("", "asc")]
        public async Task SearchOrdered(
            string orderBy,
            string order
            )
        {
            CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();

            var exampleCategoriesList =
                _fixture.GetExampleCategoriesList(10);

            await dbContext.AddRangeAsync(exampleCategoriesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var repository = new CategoryRepository(dbContext);
            var searchOrder = order.ToLower() == "asc" ? SearchOrder.Asc : SearchOrder.Desc;
            var searchInput = new ListCategoriesInput(1, 20, "", orderBy, searchOrder);

            var usecase = new UseCase.ListCategories(repository);
            var output = await usecase.Handle(searchInput, CancellationToken.None);

            var expectedOrderedList = _fixture.CloneCategoriesListOrdered(
                exampleCategoriesList,
                orderBy,
                searchOrder
             );

            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.PerPage.Should().Be(searchInput.PerPage);
            output.Total.Should().Be(expectedOrderedList.Count);
            output.Items.Should().HaveCount(expectedOrderedList.Count);

            for (int indice = 0; indice < expectedOrderedList.Count; indice++)
            {
                var expectedItem = expectedOrderedList[indice];
                var outputItem = output.Items[indice];
                expectedItem.Should().NotBeNull();
                outputItem.Should().NotBeNull();
                outputItem.Name.Should().Be(expectedItem.Name);
                outputItem.Id.Should().Be(expectedItem.Id);
                outputItem.Description.Should().Be(expectedItem.Description);
                outputItem.IsActive.Should().Be(expectedItem.IsActive);
                outputItem.CreatedAt.Should().Be(expectedItem.CreatedAt);
            }
        }
    }
}
