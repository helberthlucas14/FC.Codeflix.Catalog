using FC.Codeflix.Catalog.Integration.Tests.Base;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;
namespace FC.Codeflix.Catalog.Integration.Tests.Application.UseCases.Category.Common
{
    public class CategoryUseCaseBaseFixture : BaseFixture
    {

        public string GetValidCategoryName()
        {
            var categoryName = "";
            while (categoryName.Length < 3)
            {
                categoryName = Faker.Commerce.Categories(1)[0];
            }

            if (categoryName.Length > 255)
                categoryName = categoryName[..255];

            return categoryName;
        }

        public string GetValidCategoryDescription()
        {
            var categoryDescription = "";

            if (categoryDescription.Length > 10_000)
                categoryDescription = categoryDescription[..10_000];

            return categoryDescription;
        }

        public bool GetRandomBoolean()
            => new Random().NextDouble() < 0.5;

        public DomainEntity.Category GetExampleCategory()
             => new(GetValidCategoryName(),
                    GetValidCategoryDescription(),
                    GetRandomBoolean()
                );

        public List<DomainEntity.Category> GetExampleCategoriesList(int length = 10)
            => Enumerable.Range(1, length)
              .Select(_ => GetExampleCategory())
              .ToList();

    }
}
