using FC.Codeflix.Catalog.Integration.Tests.Application.UseCases.CastMember.Common;
using Xunit;

namespace FC.Codeflix.Catalog.Integration.Tests.Application.UseCases.CastMember.CreateCastMember;

[CollectionDefinition(nameof(CreateCastMemberTestFixture))]
public class CreateCastMemberTestFixtureCollection
    : ICollectionFixture<CreateCastMemberTestFixture>
{
}

public class CreateCastMemberTestFixture
    : CastMemberUseCasesBaseFixture
{
}
