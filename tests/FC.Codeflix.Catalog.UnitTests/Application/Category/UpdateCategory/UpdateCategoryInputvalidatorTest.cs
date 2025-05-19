using FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using FluentAssertions;

namespace FC.Codeflix.Catalog.UnitTests.Application.Category.UpdateCategory
{
    [Collection(nameof(UpdateCategoryTextFixture))]
    public class UpdateCategoryInputvalidatorTest
    {
        private readonly UpdateCategoryTextFixture _fixture;

        public UpdateCategoryInputvalidatorTest(UpdateCategoryTextFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = nameof(DontValidateWhenEmptyGuid))]
        [Trait("Application", "UpdateCategoryInputValidator - Use Case")]
        public void DontValidateWhenEmptyGuid()
        {
            var input = _fixture.GetValidInput(Guid.Empty);

            var validator = new UpdateCategoryInputValidator();

            var validationResult = validator.Validate(input);

            validationResult.Should().NotBeNull();
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().HaveCount(1);
            validationResult.Errors[0].ErrorMessage
                            .Should().Be("'Id' must not be empty.");
        }

        [Fact(DisplayName = nameof(ValidateWhenValid))]
        [Trait("Application", "UpdateCategoryInputValidator - Use Case")]
        public void ValidateWhenValid()
        {
            var input = _fixture.GetValidInput();

            var validator = new UpdateCategoryInputValidator();

            var validationResult = validator.Validate(input);

            validationResult.Should().NotBeNull();
            validationResult.IsValid.Should().BeTrue();
            validationResult.Errors.Should().HaveCount(0);
        }
    }
}
