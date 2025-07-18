using FC.Codeflix.Catalog.EndToEndTests.Api.Category.Common;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.DeleteCategory
{
    public class DeleteCategoryApiTestFixture : CategoryBaseFixture
    { }

    [CollectionDefinition(nameof(DeleteCategoryApiTestFixture))]
    public class DeleteCategoryApiTestFixtureCollection : ICollectionFixture<DeleteCategoryApiTestFixture> 
    { }
}
