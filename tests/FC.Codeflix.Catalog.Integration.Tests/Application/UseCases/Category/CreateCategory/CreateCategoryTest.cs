using FC.Codeflix.Catalog.Application;
using FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.Infra.Data.EF;
using FC.Codeflix.Catalog.Infra.Data.EF.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using UseCase = FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
namespace FC.Codeflix.Catalog.Integration.Tests.Application.UseCases.Category.CreateCategory
{
    [Collection(nameof(CreateCategoryTestFixture))]
    public class CreateCategoryTest
    {
        private readonly CreateCategoryTestFixture _fixture;

        public CreateCategoryTest(CreateCategoryTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = nameof(CreateCategory))]
        [Trait("Integration/Application", "CreateCategory - Use Cases")]
        public async void CreateCategory()
        {
            var dbContext = _fixture.CreateDbContext();
            var repository = new CategoryRepository(dbContext);
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var eventPublisher = new DomainEventPublisher(serviceProvider);
            var unitOfWork = new UnitOfWork(
                dbContext,
                eventPublisher,
                serviceProvider.GetRequiredService<ILogger<UnitOfWork>>());

            var useCase = new UseCase.CreateCategory(
                    repository, unitOfWork
                    );

            var input = _fixture.GetInput();
            var output = await useCase.Handle(input, CancellationToken.None);

            var dbCategory = await (_fixture.CreateDbContext(true))
                .Categories.FindAsync(output.Id);

            dbCategory.Should().NotBeNull();
            dbCategory.Name.Should().Be(input.Name);
            dbCategory.Description.Should().Be(input.Description);
            dbCategory.IsActive.Should().Be(input.IsActive);
            dbCategory.CreatedAt.Should().Be(output.CreatedAt);

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(input.Description);
            output.IsActive.Should().Be(input.IsActive);
            output.Id.Should().NotBeEmpty();
            output.CreatedAt.Should().NotBeSameDateAs(default);
        }



        [Fact(DisplayName = nameof(CreateCategoryWithOnlyName))]
        [Trait("Integration/Application", "CreateCategory - Use Cases")]
        public async void CreateCategoryWithOnlyName()
        {
            var dbContext = _fixture.CreateDbContext();
            var repositoryMock = new CategoryRepository(dbContext);
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var eventPublisher = new DomainEventPublisher(serviceProvider);
            var unitOfWork = new UnitOfWork(
                dbContext,
                eventPublisher,
                serviceProvider.GetRequiredService<ILogger<UnitOfWork>>());

            var useCase = new UseCase.CreateCategory(
                    repositoryMock, unitOfWork
                    );

            var input = new CreateCategoryInput(
                _fixture.GetValidCategoryName());

            var output = await useCase.Handle(input, CancellationToken.None);


            var dbCategory = await (_fixture.CreateDbContext(true))
                   .Categories.FindAsync(output.Id);

            dbCategory.Should().NotBeNull();
            dbCategory.Name.Should().Be(input.Name);
            dbCategory.Description.Should().Be("");
            dbCategory.IsActive.Should().BeTrue();
            dbCategory.CreatedAt.Should().Be(output.CreatedAt);

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be("");
            output.IsActive.Should().BeTrue();
            output.Id.Should().NotBeEmpty();
            output.CreatedAt.Should().NotBeSameDateAs(default);
        }


        [Fact(DisplayName = nameof(CreateCategoryWithOnlyNameAndDescription))]
        [Trait("Integration/Application", "CreateCategory - Use Cases")]
        public async void CreateCategoryWithOnlyNameAndDescription()
        {
            var dbContext = _fixture.CreateDbContext();
            var repository = new CategoryRepository(dbContext);
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var eventPublisher = new DomainEventPublisher(serviceProvider);
            var unitOfWork = new UnitOfWork(
                dbContext,
                eventPublisher,
                serviceProvider.GetRequiredService<ILogger<UnitOfWork>>());

            var useCase = new UseCase.CreateCategory(
                    repository, unitOfWork
                    );

            var input = new CreateCategoryInput(
                _fixture.GetValidCategoryName(),
                _fixture.GetValidCategoryDescription()
                );

            var output = await useCase.Handle(input, CancellationToken.None);

            var dbCategory = await (_fixture.CreateDbContext(true))
                  .Categories.FindAsync(output.Id);

            dbCategory.Should().NotBeNull();
            dbCategory.Name.Should().Be(input.Name);
            dbCategory.Description.Should().Be(input.Description);
            dbCategory.IsActive.Should().BeTrue();
            dbCategory.CreatedAt.Should().Be(output.CreatedAt);


            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(input.Description);
            output.IsActive.Should().BeTrue();
            output.Id.Should().NotBeEmpty();
            output.CreatedAt.Should().NotBeSameDateAs(default);
        }

        [Theory(DisplayName = nameof(ThrowWhenCantInstiateCategory))]
        [Trait("Integration/Application", "CreateCategory - Use Cases")]
        [MemberData(
         nameof(CreateCategoryTestDataGenerator.GetInvalidInputs),
         parameters: 4,
         MemberType = typeof(CreateCategoryTestDataGenerator))]
        public async void ThrowWhenCantInstiateCategory(CreateCategoryInput input, string exeception)
        {
            var dbContext = _fixture.CreateDbContext();
            var repository = new CategoryRepository(dbContext);
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var eventPublisher = new DomainEventPublisher(serviceProvider);
            var unitOfWork = new UnitOfWork(
                dbContext,
                eventPublisher,
                serviceProvider.GetRequiredService<ILogger<UnitOfWork>>());

            var useCase = new UseCase.CreateCategory(
                  repository,
                   unitOfWork
            );

            Func<Task> task
                = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should()
                  .ThrowAsync<EntityValidationException>()
                  .WithMessage(exeception);

            var dbCategories = _fixture.CreateDbContext(true).Categories
                .AsNoTracking()
                .ToList();

            dbCategories.Should().HaveCount(0);
        }

    }
}
