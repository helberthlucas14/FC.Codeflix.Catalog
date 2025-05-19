using FC.Codeflix.Catalog.Application.Exceptions;
using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Exceptions;
using FluentAssertions;
using Moq;
using UseCase = FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;

namespace FC.Codeflix.Catalog.UnitTests.Application.Category.UpdateCategory
{
    [Collection(nameof(UpdateCategoryTextFixture))]
    public class UpdateCategoryTest
    {
        private readonly UpdateCategoryTextFixture _fixture;

        public UpdateCategoryTest(UpdateCategoryTextFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory(DisplayName = nameof(UpdateCategory))]
        [Trait("Application", "UpdateCategory - Use Cases")]
        [MemberData(
            nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
            parameters: 10,
            MemberType = typeof(UpdateCategoryTestDataGenerator))]
        public async Task UpdateCategory(DomainEntity.Category exampleCategory, UpdateCategoryInput input)
        {
            var repositoryMock = _fixture.GetRepositoryMock();
            var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            repositoryMock.Setup(x => x.Get(
                exampleCategory.Id,
                It.IsAny<CancellationToken>())
            ).ReturnsAsync(exampleCategory);

            var useCase = new UseCase.UpdateCategory(
                repositoryMock.Object,
                unitOfWorkMock.Object
                );

            CategoryModelOutput outPut = await useCase.Handle(input, CancellationToken.None);

            outPut.Should().NotBeNull();
            outPut.Name.Should().Be(input.Name);
            outPut.Description.Should().Be(input.Description);
            outPut.IsActive.Should().Be((bool)input.IsActive!);

            repositoryMock.Verify(x =>
                x.Get(exampleCategory.Id,
                It.IsAny<CancellationToken>()),
                Times.Once);

            repositoryMock.Verify(x =>
               x.Update(
               exampleCategory,
               It.IsAny<CancellationToken>()),
               Times.Once);

            unitOfWorkMock.Verify(x =>
               x.Commit(It.IsAny<CancellationToken>()),
              Times.Once);

        }

        [Fact(DisplayName = nameof(ThrownWhenNotFoundUpdateCategory))]
        [Trait("Application", "UpdateCategory - Use Cases")]
        public async Task ThrownWhenNotFoundUpdateCategory()
        {
            var repositoryMock = _fixture.GetRepositoryMock();
            var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            var input = _fixture.GetValidInput();
            repositoryMock.Setup(x => x.Get(
                 input.Id,
                It.IsAny<CancellationToken>())
            ).ThrowsAsync(new NotFoundException($"Category '{input.Id}' not found"));
            var useCase = new UseCase.UpdateCategory(
                repositoryMock.Object,
                unitOfWorkMock.Object
                );

            var task = async () => await useCase.Handle(input, CancellationToken.None);
            await task.Should().ThrowAsync<NotFoundException>();

            repositoryMock.Verify(x =>
                x.Get(input.Id,
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Theory(DisplayName = nameof(UpdateCategoryWithoutProvidingIsActive))]
        [Trait("Application", "UpdateCategory - Use Cases")]
        [MemberData(
             nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
             parameters: 10,
             MemberType = typeof(UpdateCategoryTestDataGenerator))]
        public async Task UpdateCategoryWithoutProvidingIsActive(DomainEntity.Category exampleCategory, UpdateCategoryInput exampleInput)
        {
            var repositoryMock = _fixture.GetRepositoryMock();
            var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

            var input = new UpdateCategoryInput(
                exampleInput.Id,
                exampleInput.Name,
                exampleInput.Description
                );

            repositoryMock.Setup(x => x.Get(
                exampleCategory.Id,
                It.IsAny<CancellationToken>())
            ).ReturnsAsync(exampleCategory);

            var useCase = new UseCase.UpdateCategory(
                repositoryMock.Object,
                unitOfWorkMock.Object
                );

            CategoryModelOutput outPut = await useCase.Handle(input, CancellationToken.None);

            outPut.Should().NotBeNull();
            outPut.Name.Should().Be(input.Name);
            outPut.Description.Should().Be(input.Description);
            outPut.IsActive.Should().Be(exampleCategory.IsActive);

            repositoryMock.Verify(x =>
                x.Get(exampleCategory.Id,
                It.IsAny<CancellationToken>()),
                Times.Once);

            repositoryMock.Verify(x =>
               x.Update(
               exampleCategory,
               It.IsAny<CancellationToken>()),
               Times.Once);

            unitOfWorkMock.Verify(x =>
               x.Commit(It.IsAny<CancellationToken>()),
              Times.Once);

        }

        [Theory(DisplayName = nameof(UpdateCategoryWithoutProvidingDescription))]
        [Trait("Application", "UpdateCategory - Use Cases")]
        [MemberData(
     nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
     parameters: 10,
     MemberType = typeof(UpdateCategoryTestDataGenerator))]
        public async Task UpdateCategoryWithoutProvidingDescription(DomainEntity.Category exampleCategory, UpdateCategoryInput exampleInput)
        {
            var repositoryMock = _fixture.GetRepositoryMock();
            var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

            var input = new UpdateCategoryInput(
                exampleInput.Id,
                exampleInput.Name,
                null,
                exampleInput.IsActive
                );

            repositoryMock.Setup(x => x.Get(
                exampleCategory.Id,
                It.IsAny<CancellationToken>())
            ).ReturnsAsync(exampleCategory);

            var useCase = new UseCase.UpdateCategory(
                repositoryMock.Object,
                unitOfWorkMock.Object
                );

            CategoryModelOutput outPut = await useCase.Handle(input, CancellationToken.None);

            outPut.Should().NotBeNull();
            outPut.Name.Should().Be(input.Name);
            outPut.Description.Should().Be(exampleCategory.Description);
            outPut.IsActive.Should().Be((bool)input.IsActive!);

            repositoryMock.Verify(x =>
                x.Get(exampleCategory.Id,
                It.IsAny<CancellationToken>()),
                Times.Once);

            repositoryMock.Verify(x =>
               x.Update(
               exampleCategory,
               It.IsAny<CancellationToken>()),
               Times.Once);

            unitOfWorkMock.Verify(x =>
               x.Commit(It.IsAny<CancellationToken>()),
              Times.Once);

        }


        [Theory(DisplayName = nameof(UpdateCategoryOnlyName))]
        [Trait("Application", "UpdateCategory - Use Cases")]
        [MemberData(
             nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
             parameters: 10,
             MemberType = typeof(UpdateCategoryTestDataGenerator)
         )]
        public async Task UpdateCategoryOnlyName(DomainEntity.Category exampleCategory, UpdateCategoryInput exampleInput)
        {
            var repositoryMock = _fixture.GetRepositoryMock();
            var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

            var input = new UpdateCategoryInput(
                exampleInput.Id,
                exampleInput.Name);

            repositoryMock.Setup(x => x.Get(
                exampleCategory.Id,
                It.IsAny<CancellationToken>())
            ).ReturnsAsync(exampleCategory);

            var useCase = new UseCase.UpdateCategory(
                repositoryMock.Object,
                unitOfWorkMock.Object
                );

            CategoryModelOutput outPut = await useCase.Handle(input, CancellationToken.None);

            outPut.Should().NotBeNull();
            outPut.Name.Should().Be(input.Name);
            outPut.Description.Should().Be(exampleCategory.Description);
            outPut.IsActive.Should().Be((bool)exampleCategory.IsActive!);

            repositoryMock.Verify(x =>
                x.Get(exampleCategory.Id,
                It.IsAny<CancellationToken>()),
                Times.Once);

            repositoryMock.Verify(x =>
               x.Update(
               exampleCategory,
               It.IsAny<CancellationToken>()),
               Times.Once);

            unitOfWorkMock.Verify(x =>
               x.Commit(It.IsAny<CancellationToken>()),
              Times.Once);

        }

        [Theory(DisplayName = nameof(ThrownWhenCantUpdateCategory))]
        [Trait("Application", "UpdateCategory - Use Cases")]
        [MemberData(
            nameof(UpdateCategoryTestDataGenerator.GetInvalidInputs),
            parameters: 12,
            MemberType = typeof(UpdateCategoryTestDataGenerator)
        )]
        public async Task ThrownWhenCantUpdateCategory(UpdateCategoryInput input, string exeception)
        {
            var exampleCategory = _fixture.GetExampleCategory();
            input.Id = exampleCategory.Id;
            var repositoryMock = _fixture.GetRepositoryMock();
            var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

            repositoryMock.Setup(x => x.Get(
                exampleCategory.Id,
                It.IsAny<CancellationToken>())
            ).ReturnsAsync(exampleCategory);

            var useCase = new UseCase.UpdateCategory(
                repositoryMock.Object,
                unitOfWorkMock.Object
                );

            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should()
                .ThrowAsync<EntityValidationException>()
                .WithMessage(exeception);

            repositoryMock.Verify(x => x.Get(
                exampleCategory.Id,
                It.IsAny<CancellationToken>()),
            Times.Once);
        }
    }
}
