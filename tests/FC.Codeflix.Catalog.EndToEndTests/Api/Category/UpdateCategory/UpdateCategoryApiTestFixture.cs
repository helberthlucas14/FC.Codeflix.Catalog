﻿using FC.Codeflix.Catalog.Api.ApiModels.Category;
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
        public UpdateCategoryApiInput GetExampleInput()
            =>  new UpdateCategoryApiInput(
                GetValidCategoryName(),
                GetValidCategoryDescription(),
                GetRandomBoolean());
    }
}
