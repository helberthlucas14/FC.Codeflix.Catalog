﻿using FC.Codeflix.Catalog.Infra.Data.EF.Repositories;
using UseCase = FC.Codeflix.Catalog.Application.UseCases.Category.GetCategory;
using FluentAssertions;
using FC.Codeflix.Catalog.Application.Exceptions;

namespace FC.Codeflix.Catalog.Integration.Tests.Application.UseCases.Category.GetCategory
{
    [Collection(nameof(GetCategoryTestFixture))]
    public class GetCategoryTest
    {
        private readonly GetCategoryTestFixture _fixture;

        public GetCategoryTest(GetCategoryTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = nameof(GetCategory))]
        [Trait("Integration/Application", "GetCategory - Use Cases")]
        public async Task GetCategory()
        {
            var exampleCategory = _fixture.GetExampleCategory();
            var dbContext = _fixture.CreateDbContext();
            dbContext.Categories.Add(exampleCategory);
            dbContext.SaveChanges();
            var repository = new CategoryRepository(dbContext);

            var input = new UseCase.GetCategoryInput(exampleCategory.Id);
            var useCase = new UseCase.GetCategory(repository);

            var output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Name.Should().Be(exampleCategory.Name);
            output.Description.Should().Be(exampleCategory.Description);
            output.IsActive.Should().Be(exampleCategory.IsActive);
            output.Id.Should().Be(exampleCategory.Id);
            output.CreatedAt.Should().Be(exampleCategory.CreatedAt);
        }

        [Fact(DisplayName = nameof(NotFoundExceptionWhenCategoryDoesntExist))]
        [Trait("Integration/Application", "GetCategory - Use Cases")]
        public async Task NotFoundExceptionWhenCategoryDoesntExist()
        {
            var dbContext = _fixture.CreateDbContext();
            var repository = new CategoryRepository(dbContext);
            var exampleGuid = Guid.NewGuid();

            var input = new UseCase.GetCategoryInput(exampleGuid);
            var useCase = new UseCase.GetCategory(repository);

            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Category '{input.Id}' not found.");
        }
    }
}
