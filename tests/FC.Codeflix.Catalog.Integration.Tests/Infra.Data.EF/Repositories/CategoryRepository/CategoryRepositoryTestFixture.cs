﻿using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using FC.Codeflix.Catalog.Integration.Tests.Base;

namespace FC.Codeflix.Catalog.Integration.Tests.Infra.Data.EF.Repositories.CategoryRepository
{
    [CollectionDefinition(nameof(CategoryRepositoryTestFixture))]
    public class CategoryRepositoryTestFixtureCollection
        : ICollectionFixture<CategoryRepositoryTestFixture>
    { }

    public class CategoryRepositoryTestFixture : BaseFixture
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

        public List<Category> GetExampleCategoriesListWithNames(List<string> names)
            => names.Select(name =>
            {
                var category = GetExampleCategory();
                category.Update(name);
                return category;
            }).ToList();

        public List<Category> CloneCategoriesListOrdered(
            List<Category> categoriesList,
            string orderBy,
            SearchOrder order)
        {
            var listClone = new List<Category>(categoriesList);
            var ordenredEnumerable = (orderBy.ToLower(), order) switch
            {
                ("id", SearchOrder.Asc) => listClone.OrderBy(x => x.Id),
                ("id", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Id),
                ("name", SearchOrder.Asc) => listClone.OrderBy(x => x.Name),
                ("name", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Name),
                ("createdAt", SearchOrder.Asc) => listClone.OrderBy(x => x.CreatedAt),
                ("createdAt", SearchOrder.Desc) => listClone.OrderByDescending(x => x.CreatedAt),
                _ => listClone.OrderBy(x => x.Name),
            };
            return ordenredEnumerable.ToList();
        }
    }
}
