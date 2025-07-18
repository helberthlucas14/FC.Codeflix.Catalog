using FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using FC.Codeflix.Catalog.EndToEndTests.Api.Category.Common;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.UpdateCategory
{
    [CollectionDefinition(nameof(UpdateCategoryApiTestFixture))]
    public class UpdateCategoryApiTestFixtureCollection
    : ICollectionFixture<UpdateCategoryApiTestFixture>
    {

    }
    public class UpdateCategoryApiTestFixture : CategoryBaseFixture
    {
        public UpdateCategoryInput GetExampleInput(Guid? id )
            =>  new UpdateCategoryInput(
                 id ?? Guid.NewGuid(),
                GetValidCategoryName(),
                GetValidCategoryDescription(),
                GetRandomBoolean());
    }
}
