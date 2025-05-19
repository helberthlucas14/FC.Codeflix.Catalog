using FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using FC.Codeflix.Catalog.UnitTests.Application.Category.Common;

namespace FC.Codeflix.Catalog.UnitTests.Application.Category.CreateCategory
{
    public class CreateCategoryTestFixture
        : CategoryUseCaseBaseFixture
    {

        public CreateCategoryInput GetInput() =>
            new(
                GetValidCategoryName(),
                GetValidCategoryDescription(),
                GetRandomBoolean()
                );

        public CreateCategoryInput GetInvalidInputShortName()
        {
            var invalidInputShortName = GetInput();
            invalidInputShortName.Name = invalidInputShortName.Name.Substring(0, 2);
            return invalidInputShortName;
        }

        public CreateCategoryInput GetInvalidInputTooLongName()
        {
            var invalidInputLongName = GetInput();
            var tooLongNameForCategory = "";

            while (tooLongNameForCategory.Length <= 255)
                tooLongNameForCategory += $"{tooLongNameForCategory} {Faker.Commerce.ProductName}";

            invalidInputLongName.Name = tooLongNameForCategory;

            return invalidInputLongName;
        }

        public CreateCategoryInput GetInvalidInputCategoryNull()
        {
            var invalidInputDescriptionNull = GetInput();
            invalidInputDescriptionNull.Description = null!;
            return invalidInputDescriptionNull;
        }

        public CreateCategoryInput GetInvalidInputTooLongDescription()
        {
            var invalidInputTooLongDescription = GetInput();
            var tooLongDescriptionForCategory = Faker.Commerce.ProductDescription();
            while (tooLongDescriptionForCategory.Length <= 10_000)
                tooLongDescriptionForCategory += $"{tooLongDescriptionForCategory} {Faker.Commerce.ProductDescription}";

            invalidInputTooLongDescription.Description = tooLongDescriptionForCategory;

            return invalidInputTooLongDescription;
        }
    }


    [CollectionDefinition(nameof(CreateCategoryTestFixture))]
    public class CreateCategoryTestFixtureCollection
    : ICollectionFixture<CreateCategoryTestFixture>
    {

    }
}
