using Bogus;

namespace FC.Codeflix.Catalog.Integration.Tests.Infra.Data.EF.Base
{
    public class BaseFixture
    {
        protected Faker Faker { get; set; }
        public BaseFixture() => Faker = new Faker("pt_BR");
    }
}
