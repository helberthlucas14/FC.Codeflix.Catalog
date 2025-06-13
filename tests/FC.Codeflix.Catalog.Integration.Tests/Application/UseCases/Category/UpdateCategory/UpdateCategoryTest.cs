using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;
using UseCase = FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using FluentAssertions;
using FC.Codeflix.Catalog.Infra.Data.EF.Repositories;
using FC.Codeflix.Catalog.Infra.Data.EF;
using FC.Codeflix.Catalog.Application.Exceptions;
using FC.Codeflix.Catalog.Domain.Exceptions;



namespace FC.Codeflix.Catalog.Integration.Tests.Application.UseCases.Category.UpdateCategory
{
    [Collection(nameof(UpdateCategoryTestFixture))]
    public class UpdateCategoryTest
    {
        private readonly UpdateCategoryTestFixture _fixture;

        public UpdateCategoryTest(UpdateCategoryTestFixture fixture) => _fixture = fixture;


        [Theory(DisplayName = nameof(UpdateCategory))]
        [Trait("Integration/Application", "UpdateCategory - Use Cases")]
        [MemberData(
        nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
        parameters: 10,
        MemberType = typeof(UpdateCategoryTestDataGenerator))]
        public async Task UpdateCategory(DomainEntity.Category exampleCategory, UpdateCategoryInput input)
        {
            var dbContext = _fixture.CreateDbContext();
            await dbContext.AddRangeAsync(_fixture.GetExampleCategoriesList());
            var trackingInfo = await dbContext.AddAsync(exampleCategory);
            dbContext.SaveChanges();
            trackingInfo.State = Microsoft.EntityFrameworkCore.EntityState.Detached;

            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);

            var useCase = new UseCase.UpdateCategory(
                repository,
                unitOfWork
                );

            CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(input.Description);
            output.IsActive.Should().Be((bool)input.IsActive!);

            var dbCategory = await (_fixture.CreateDbContext(true))
                                .Categories.FindAsync(output.Id);

            dbCategory.Should().NotBeNull();
            dbCategory.Name.Should().Be(input.Name);
            dbCategory.Description.Should().Be(input.Description);
            dbCategory.IsActive.Should().Be((bool)input.IsActive);
            dbCategory.CreatedAt.Should().Be(output.CreatedAt);
        }

        [Theory(DisplayName = nameof(UpdateCategoryWithoutProvidingIsActive))]
        [Trait("Integration/Application", "UpdateCategory - Use Cases")]
        [MemberData(
         nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
         parameters: 10,
         MemberType = typeof(UpdateCategoryTestDataGenerator))]
        public async Task UpdateCategoryWithoutProvidingIsActive(DomainEntity.Category exampleCategory, UpdateCategoryInput exampleInput)
        {

            var input = new UpdateCategoryInput(
                exampleInput.Id,
                exampleInput.Name,
                exampleInput.Description
                );

            var dbContext = _fixture.CreateDbContext();
            await dbContext.AddRangeAsync(_fixture.GetExampleCategoriesList());
            var trackingInfo = await dbContext.AddAsync(exampleCategory);
            dbContext.SaveChanges();
            trackingInfo.State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);


            var useCase = new UseCase.UpdateCategory(
                repository,
                unitOfWork
                );

            CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(input.Description);
            output.IsActive.Should().Be(exampleCategory.IsActive);


            var dbCategory = await (_fixture.CreateDbContext(true))
                                .Categories.FindAsync(output.Id);

            dbCategory.Should().NotBeNull();
            dbCategory.Name.Should().Be(output.Name);
            dbCategory.Description.Should().Be(output.Description);
            dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
            dbCategory.CreatedAt.Should().Be(output.CreatedAt);
        }

        [Theory(DisplayName = nameof(UpdateCategoryOnlyName))]
        [Trait("Integration/Application", "UpdateCategory - Use Cases")]
        [MemberData(
             nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
             parameters: 10,
             MemberType = typeof(UpdateCategoryTestDataGenerator)
         )]
        public async Task UpdateCategoryOnlyName(DomainEntity.Category exampleCategory, UpdateCategoryInput exampleInput)
        {
            var input = new UpdateCategoryInput(
                         exampleInput.Id,
                         exampleInput.Name);

            var dbContext = _fixture.CreateDbContext();
            await dbContext.AddRangeAsync(_fixture.GetExampleCategoriesList());
            var trackingInfo = await dbContext.AddAsync(exampleCategory);
            dbContext.SaveChanges();
            trackingInfo.State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);

            var useCase = new UseCase.UpdateCategory(
                            repository,
                            unitOfWork
                        );

            CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(exampleCategory.Description);
            output.IsActive.Should().Be((bool)exampleCategory.IsActive!);

            var dbCategory = await (_fixture.CreateDbContext(true))
                    .Categories.FindAsync(output.Id);

            dbCategory.Should().NotBeNull();
            dbCategory.Name.Should().Be(input.Name);
            dbCategory.Description.Should().Be(exampleCategory.Description);
            dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
            dbCategory.CreatedAt.Should().Be(output.CreatedAt);
        }


        [Fact(DisplayName = nameof(ThrownWhenNotFoundUpdateCategory))]
        [Trait("Integration/Application", "UpdateCategory - Use Cases")]
        public async Task ThrownWhenNotFoundUpdateCategory()
        {
            var dbContext = _fixture.CreateDbContext();
            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);

            var input = _fixture.GetValidInput();

            var useCase = new UseCase.UpdateCategory(
                repository,
                unitOfWork
                );

            var task = async () => await useCase.Handle(input, CancellationToken.None);
            await task.Should().ThrowAsync<NotFoundException>();
        }


        [Theory(DisplayName = nameof(UpdateThrowsWhenCanIntantiateCategory))]
        [Trait("Integration/Application", "UpdateCategory - Use Cases")]
        [MemberData(
             nameof(UpdateCategoryTestDataGenerator.GetInvalidInputs),
             parameters: 6,
             MemberType = typeof(UpdateCategoryTestDataGenerator)
         )]
        public async Task UpdateThrowsWhenCanIntantiateCategory(
            UpdateCategoryInput input,
            string expectedExceptionMessage
            )
        {
            var dbContext = _fixture.CreateDbContext();
            var exampleCategories = _fixture.GetExampleCategoriesList();
            await dbContext.AddRangeAsync(exampleCategories);
            dbContext.SaveChanges();
            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);
            var useCase = new UseCase.UpdateCategory(
                repository,
                unitOfWork
                );

            input.Id = exampleCategories[0].Id;
            var task = async () => await useCase.Handle(input, CancellationToken.None);
            await task.Should().ThrowAsync<EntityValidationException>()
                .WithMessage(expectedExceptionMessage);
        }
    }
}
