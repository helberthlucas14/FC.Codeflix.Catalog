using FC.Codeflix.Catalog.Integration.Tests.Application.UseCases.Category.Common;

namespace FC.Codeflix.Catalog.Integration.Tests.Application.UseCases.Category.DeleteCategory
{
    [CollectionDefinition(nameof(DeleteCategoryTestFixture))]
    public class DeleteCategoryTestFixtureCollection 
        : ICollectionFixture<DeleteCategoryTestFixture>
    { }

    public class DeleteCategoryTestFixture : CategoryUseCaseBaseFixture
    {

    }
}
