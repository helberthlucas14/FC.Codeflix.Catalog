using FC.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using FC.Codeflix.Catalog.Integration.Tests.Application.UseCases.Category.Common;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.Integration.Tests.Application.UseCases.Category.ListCategories
{
    [CollectionDefinition(nameof(ListCategoriesTestFixture))]
    public class ListCategoriesTestFixtureCollection
        : ICollectionFixture<ListCategoriesTestFixture>
    { }

    public class ListCategoriesTestFixture : CategoryUseCaseBaseFixture
    {
        public List<DomainEntity.Category> GetExampleCategoriesListWithNames(List<string> names)
            => names.Select(name =>
            {
                var category = GetExampleCategory();
                category.Update(name);
                return category;
            }).ToList();

        public List<DomainEntity.Category> CloneCategoriesListOrdered(
          List<DomainEntity.Category> categoriesList,
          string orderBy,
          SearchOrder order)
        {
            var listClone = new List<DomainEntity.Category>(categoriesList);
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
