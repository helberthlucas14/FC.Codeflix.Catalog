using FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.UnitTests.Application.Category.Common;

namespace FC.Codeflix.Catalog.UnitTests.Application.Category.UpdateCategory
{

    [CollectionDefinition(nameof(UpdateCategoryTextFixture))]
    public class GetCategoryTestFixtureCollection :
    ICollectionFixture<UpdateCategoryTextFixture>
    { }
    public class UpdateCategoryTextFixture : CategoryUseCaseBaseFixture
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
