using FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using FC.Codeflix.Catalog.Integration.Tests.Application.UseCases.Category.Common;

namespace FC.Codeflix.Catalog.Integration.Tests.Application.UseCases.Category.UpdateCategory
{
    [CollectionDefinition(nameof(UpdateCategoryTestFixture))]
    public class UpdateCategoryTestFixtureCollection
        : ICollectionFixture<UpdateCategoryTestFixture>
    {

    }

    public class UpdateCategoryTestFixture : CategoryUseCaseBaseFixture
    {
        public UpdateCategoryInput GetValidInput(Guid? id = null) =>
                new(id ?? Guid.NewGuid(),
                   GetValidCategoryName(),
                   GetValidCategoryDescription(),
                   GetRandomBoolean());

        public UpdateCategoryInput GetInvalidInputShortName()
        {
            var invalidInputShortName = GetValidInput();
            invalidInputShortName.Name = invalidInputShortName.Name.Substring(0, 2);
            return invalidInputShortName;
        }

        public UpdateCategoryInput GetInvalidInputTooLongName()
        {
            var invalidInputLongName = GetValidInput();
            var tooLongNameForCategory = "";

            while (tooLongNameForCategory.Length <= 255)
                tooLongNameForCategory += $"{tooLongNameForCategory} {Faker.Commerce.ProductName}";

            invalidInputLongName.Name = tooLongNameForCategory;

            return invalidInputLongName;
        }

        public UpdateCategoryInput GetInvalidInputTooLongDescription()
        {
            var invalidInputTooLongDescription = GetValidInput();
            var tooLongDescriptionForCategory = Faker.Commerce.ProductDescription();
            while (tooLongDescriptionForCategory.Length <= 10_000)
                tooLongDescriptionForCategory += $"{tooLongDescriptionForCategory} {Faker.Commerce.ProductDescription}";

            invalidInputTooLongDescription.Description = tooLongDescriptionForCategory;

            return invalidInputTooLongDescription;
        }
    }

}
