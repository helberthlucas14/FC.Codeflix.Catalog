﻿using FC.Codeflix.Catalog.Application.UseCases.Category.GetCategory;
using FluentAssertions;

namespace FC.Codeflix.Catalog.UnitTests.Application.Category.GetCategory
{
    [Collection(nameof(GetCategoryTestFixture))]
    public class GetCategoryInputValidatorTest
    {
        private readonly GetCategoryTestFixture _fixture;

        public GetCategoryInputValidatorTest(GetCategoryTestFixture fixture) => _fixture = fixture;

        [Fact(DisplayName = nameof(ValidationOk))]
        [Trait("Application", "GetCategoryInputValidationTest - UseCases")]
        public void ValidationOk()
        {
            var validInput = new GetCategoryInput(Guid.NewGuid());
            var validator = new GetCategoryInputValidator();
            var validationResult = validator.Validate(validInput);

            validationResult.Should().NotBeNull();
            validationResult.IsValid.Should().BeTrue();
            validationResult.Errors.Should().HaveCount(0);
        }

        [Fact(DisplayName = nameof(InvalidWhenEmptyGuidId))]
        [Trait("Application", "GetCategoryInputValidationTest - UseCases")]
        public void InvalidWhenEmptyGuidId()
        {
            var InvalidInput = new GetCategoryInput(Guid.Empty);
            var validator = new GetCategoryInputValidator();
            var validationResult = validator.Validate(InvalidInput);

            validationResult.Should().NotBeNull();
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().HaveCount(1);
            validationResult.Errors[0].ErrorMessage
                .Should().Be("'Id' must not be empty.");
        }
    }
}
