using FC.Codeflix.Catalog.Api.ApiModels.Response;
using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Application.UseCases.Category.ListCategories;
using FC.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using FC.Codeflix.Catalog.EndToEndTests.Extensions.DateTime;
using FC.Codeflix.Catalog.EndToEndTests.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.ListCategories
{

    [Collection(nameof(ListCategoriesApiTestFixture))]
    public class ListCategoriesApiTest : IDisposable
    {
        private readonly ListCategoriesApiTestFixture _fixture;


        public void Dispose() => _fixture.CleanPersistence();

        [Fact(DisplayName = nameof(ListCategoriesAndTotalByDefault))]
        [Trait("EndToEnd/API", "Category/List - Endpoints")]
        public async Task ListCategoriesAndTotalByDefault()
        {
            var defaultPerPage = 15;
            var exampleCategoryList = _fixture.GetExampleCategoriesList(20);
            await _fixture.Persistence.InsertList(exampleCategoryList);

            var (response, output) = await _fixture.ApiClient
          .Get<TestApiResponseList<CategoryModelOutput>>("/categories");

            response.Should().NotBeNull();
            response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();
            output.Meta.Should().NotBeNull();
            output.Data.Should().NotBeNull();
            output.Meta.Total.Should().Be(exampleCategoryList.Count);
            output.Meta.CurrentPage.Should().Be(1);
            output.Meta.PerPage.Should().Be(defaultPerPage);
            output.Data.Should().HaveCount(defaultPerPage);

            foreach (CategoryModelOutput category in output.Data)
            {
                var dbCategory = exampleCategoryList.FirstOrDefault(c => c.Id == category.Id);
                dbCategory.Should().NotBeNull();
                category.Name.Should().Be(dbCategory.Name);
                category.Description.Should().Be(dbCategory.Description);
                category.IsActive.Should().Be(dbCategory.IsActive);
                category.CreatedAt.TrimMillisseconds().Should().BeCloseTo(dbCategory.CreatedAt.TrimMillisseconds(), TimeSpan.FromSeconds(1));
            }
        }

        [Fact(DisplayName = nameof(ItemsEmptyWhenPersistenceEmpty))]
        [Trait("EndToEnd/API", "Category/List - Endpoints")]
        public async Task ItemsEmptyWhenPersistenceEmpty()
        {
            var (response, output) = await _fixture.ApiClient
             .Get<TestApiResponseList<CategoryModelOutput>>("/categories");

            response.Should().NotBeNull();
            response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();
            output.Meta.Should().NotBeNull();
            output.Data.Should().NotBeNull();
            output.Meta.Total.Should().Be(0);
            output.Data.Should().HaveCount(0);
        }


        [Fact(DisplayName = nameof(ListCategoriesAndTotal))]
        [Trait("EndToEnd/API", "Category/List - Endpoints")]
        public async Task ListCategoriesAndTotal()
        {
            var exampleCategoryList = _fixture.GetExampleCategoriesList(20);
            await _fixture.Persistence.InsertList(exampleCategoryList);
            var input = new ListCategoriesInput(page: 1, perPage: 5);

            var (response, output) = await _fixture.ApiClient
            .Get<TestApiResponseList<CategoryModelOutput>>("/categories", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();
            output.Data.Should().NotBeNull();
            output.Meta.Should().NotBeNull();
            output.Meta.CurrentPage.Should().Be(input.Page);
            output.Meta.PerPage.Should().Be(input.PerPage);
            output.Meta.Total.Should().Be(exampleCategoryList.Count);
            output.Data.Should().HaveCount(input.PerPage);

            foreach (CategoryModelOutput category in output.Data)
            {
                var dbCategory = exampleCategoryList.FirstOrDefault(c => c.Id == category.Id);
                dbCategory.Should().NotBeNull();
                category.Name.Should().Be(dbCategory.Name);
                category.Description.Should().Be(dbCategory.Description);
                category.IsActive.Should().Be(dbCategory.IsActive);
                category.CreatedAt.TrimMillisseconds().Should().BeCloseTo(dbCategory.CreatedAt.TrimMillisseconds(), TimeSpan.FromSeconds(1));
            }
        }

        [Theory(DisplayName = nameof(ListPagined))]
        [Trait("EndToEnd/API", "Category/List - Endpoints")]
        [InlineData(10, 1, 5, 5)]
        [InlineData(10, 2, 5, 5)]
        [InlineData(7, 2, 5, 2)]
        [InlineData(7, 3, 5, 0)]
        public async Task ListPagined(
            int quantityCategoryToGenerate,
            int page,
            int perPage,
            int expectedQuantityItems)
        {
            var exampleCategoryList = _fixture
                .GetExampleCategoriesList(quantityCategoryToGenerate);
            await _fixture.Persistence.InsertList(exampleCategoryList);
            var input = new ListCategoriesInput(page, perPage);

            var (response, output) = await _fixture.ApiClient
            .Get<TestApiResponseList<CategoryModelOutput>>("/categories", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();
            output.Meta.Should().NotBeNull();
            output.Data.Should().NotBeNull();
            output.Data.Should().NotBeNull();
            output.Meta.Should().NotBeNull();
            output.Meta.CurrentPage.Should().Be(input.Page);
            output.Meta.PerPage.Should().Be(input.PerPage);
            output.Data.Should().HaveCount(expectedQuantityItems);

            foreach (CategoryModelOutput category in output.Data)
            {
                var dbCategory = exampleCategoryList.FirstOrDefault(c => c.Id == category.Id);
                dbCategory.Should().NotBeNull();
                category.Name.Should().Be(dbCategory.Name);
                category.Description.Should().Be(dbCategory.Description);
                category.IsActive.Should().Be(dbCategory.IsActive);
                category.CreatedAt.TrimMillisseconds().Should().BeCloseTo(dbCategory.CreatedAt.TrimMillisseconds(), TimeSpan.FromSeconds(1));
            }
        }

        [Theory(DisplayName = nameof(SearchByText))]
        [Trait("EndToEnd/API", "Category/List - Endpoints")]
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
              int expextedQuantityTotalItems)
        {

            var exampleCategoryList =
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

            await _fixture.Persistence.InsertList(exampleCategoryList);
            var input = new ListCategoriesInput(page, perPage, search);

            var (response, output) = await _fixture.ApiClient
            .Get<TestApiResponseList<CategoryModelOutput>>("/categories", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();
            output.Data.Should().NotBeNull();
            output.Meta.Should().NotBeNull();
            output.Meta.CurrentPage.Should().Be(input.Page);
            output.Meta.PerPage.Should().Be(input.PerPage);
            output.Meta.Total.Should().Be(expextedQuantityTotalItems);
            output.Data.Should().HaveCount(expectedQuantityItemsReturned);

            foreach (CategoryModelOutput category in output.Data)
            {
                var dbCategory = exampleCategoryList.FirstOrDefault(c => c.Id == category.Id);
                dbCategory.Should().NotBeNull();
                category.Name.Should().Be(dbCategory.Name);
                category.Description.Should().Be(dbCategory.Description);
                category.IsActive.Should().Be(dbCategory.IsActive);
                category.CreatedAt.TrimMillisseconds().Should().BeCloseTo(dbCategory.CreatedAt.TrimMillisseconds(), TimeSpan.FromSeconds(1));
            }
        }

        [Theory(DisplayName = nameof(ListOrdered))]
        [Trait("EndToEnd/API", "Category/List - Endpoints")]
        [InlineData("name", "asc")]
        [InlineData("name", "desc")]
        [InlineData("id", "asc")]
        [InlineData("id", "desc")]
        [InlineData("createdAt", "asc")]
        [InlineData("createdAt", "desc")]
        [InlineData("", "asc")]
        public async Task ListOrdered(
            string orderBy,
            string order)
        {
            var exampleCategoryList = _fixture.GetExampleCategoriesList(10);
            await _fixture.Persistence.InsertList(exampleCategoryList);

            var inputOrder = order == "asc" ? SearchOrder.Asc : SearchOrder.Desc;
            var input = new ListCategoriesInput(
                page: 1,
                perPage: 20,
                sort: orderBy,
                dir: inputOrder
              );

            var (response, output) = await _fixture.ApiClient
            .Get<TestApiResponseList<CategoryModelOutput>>("/categories", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();
            output.Meta.Should().NotBeNull();
            output.Meta.CurrentPage.Should().Be(input.Page);
            output.Meta.PerPage.Should().Be(input.PerPage);
            output.Meta.Total.Should().Be(exampleCategoryList.Count);
            output.Data.Should().HaveCount(exampleCategoryList.Count);

            var expectedOrderedList = _fixture.CloneCategoriesListOrdered(
                exampleCategoryList,
                input.Sort,
                input.Dir);

            for (int i = 0; i < expectedOrderedList.Count; i++)
            {
                var outputItem = output.Data[i];
                var exampleItem = expectedOrderedList[i];
                outputItem.Should().NotBeNull();
                exampleItem.Should().NotBeNull();
                outputItem.Name.Should().Be(exampleItem.Name);
                outputItem.Id.Should().Be(exampleItem.Id);
                outputItem.Description.Should().Be(exampleItem.Description);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                outputItem.CreatedAt.TrimMillisseconds().Should().Be(exampleItem.CreatedAt.TrimMillisseconds());
            }
        }
        public ListCategoriesApiTest(ListCategoriesApiTestFixture fixture)
             => _fixture = fixture;
    }
}
