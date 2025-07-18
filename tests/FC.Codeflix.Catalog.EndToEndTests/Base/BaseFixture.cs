using Bogus;
using FC.Codeflix.Catalog.Infra.Data.EF.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FC.Codeflix.Catalog.EndToEndTests.Base
{
    public class BaseFixture
    {
        protected Faker Faker { get; set; }

        public CustomWebApplicationFactory<Program> WebApplicationFactory { get; set; }
        public HttpClient HttpClient { get; set; }
        public ApiClient ApiClient { get; set; }
        public BaseFixture()
        {
            Faker = new Faker("pt_BR");
            WebApplicationFactory = new CustomWebApplicationFactory<Program>();
            HttpClient = WebApplicationFactory.CreateClient();
            ApiClient = new ApiClient(HttpClient);
        }

        public CodeflixCatalogDbContext CreateDbContext()
        {

            var context = new CodeflixCatalogDbContext
                      (
                          new DbContextOptionsBuilder<CodeflixCatalogDbContext>()
                              .UseInMemoryDatabase("end2end-tests-db")
                              .Options
                      );

            return context;
        }

        public void CleanPersistence()
        {
            var context = CreateDbContext();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
    }
}
