﻿
using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Integration.Tests.Base;

namespace FC.Codeflix.Catalog.Integration.Tests.Infra.Data.EF.UnitOfWork
{
    [CollectionDefinition(nameof(UnitOfWorkTestFixture))]
    public class UnitOfWorkTestFixtureCollection 
        : ICollectionFixture<UnitOfWorkTestFixture> { }


    public class UnitOfWorkTestFixture 
        : BaseFixture
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

        public Category GetExampleCategory()
             => new(GetValidCategoryName(),
                    GetValidCategoryDescription(),
                    GetRandomBoolean()
                );

        public List<Category> GetExampleCategoriesList(int length = 10)
            => Enumerable.Range(1, length)
              .Select(_ => GetExampleCategory())
              .ToList();
    }
}
