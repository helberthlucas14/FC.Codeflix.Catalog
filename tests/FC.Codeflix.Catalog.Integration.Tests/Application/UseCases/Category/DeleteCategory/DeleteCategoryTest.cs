using FC.Codeflix.Catalog.Application.Exceptions;
using FC.Codeflix.Catalog.Infra.Data.EF;
using FC.Codeflix.Catalog.Infra.Data.EF.Repositories;
using FluentAssertions;
using UseCase = FC.Codeflix.Catalog.Application.UseCases.Category.DeleteCategory;
using Microsoft.EntityFrameworkCore;

namespace FC.Codeflix.Catalog.Integration.Tests.Application.UseCases.Category.DeleteCategory
{
    [Collection(nameof(DeleteCategoryTestFixture))]
    public class DeleteCategoryTest
    {
        public readonly DeleteCategoryTestFixture _fixture;

        public DeleteCategoryTest(DeleteCategoryTestFixture fixture) => _fixture = fixture;


        [Fact(DisplayName = nameof(DeleteCategory))]
        [Trait("Integration/Application", "DeleteCategoy - Use Cases")]
        public async Task DeleteCategory()
        {


            var dbContext = _fixture.CreateDbContext();
            var exampleCategory = _fixture.GetExampleCategory();
            var exampleList = _fixture.GetExampleCategoriesList(10);
            await dbContext.AddRangeAsync(exampleList);
            var trackingInfo = await dbContext.AddAsync(exampleCategory);
            dbContext.SaveChanges();
            trackingInfo.State = Microsoft.EntityFrameworkCore.EntityState.Detached;

            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);

            var input = new UseCase.DeleteCategoryInput(exampleCategory.Id);

            var useCase = new UseCase.DeleteCategory(
                repository,
                unitOfWork);

            await useCase.Handle(input, CancellationToken.None);

            var assertDbContext = _fixture.CreateDbContext(true);
            var dbCategory = await assertDbContext
                                  .Categories.FindAsync(exampleCategory.Id);

            dbCategory.Should().BeNull();
            var dbCategories = await assertDbContext.Categories.ToListAsync();
            dbCategories.Should().HaveCount(exampleList.Count);
        }

        [Fact(DisplayName = nameof(ThrownWheCategoryNotFound))]
        [Trait("Integration/Application", "DeleteCategoy - Use Cases")]
        public async Task ThrownWheCategoryNotFound()
        {
            var dbContext = _fixture.CreateDbContext();
            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);
            var exampleGuid = Guid.NewGuid();

            var input = new UseCase.DeleteCategoryInput(exampleGuid);
         
            var useCase = new UseCase.DeleteCategory(
                repository,
                unitOfWork);

            var task = async ()
                 => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Category '{exampleGuid}' not found.");
        }
    }
}
