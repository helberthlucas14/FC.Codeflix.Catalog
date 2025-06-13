using FC.Codeflix.Catalog.Integration.Tests.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Integration.Tests.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC.Codeflix.Catalog.Integration.Tests.Application.UseCases.Category.GetCategory
{

    [CollectionDefinition(nameof(GetCategoryTestFixture))]
    public class GetCategoryTestFixtureCollection
        : ICollectionFixture<GetCategoryTestFixture>
    { }

    public class GetCategoryTestFixture 
        : CategoryUseCaseBaseFixture
    {
    }
}
