using FC.Codeflix.Catalog.Infra.Data.EF.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FC.Codeflix.Catalog.Api.Configurations
{
    public static class ConnectionsConfiguration
    {
        public static IServiceCollection AddAndConfigConnections(
            this IServiceCollection services
            )
        {
            services.AddDbConnection();
            return services;
        }

        private static IServiceCollection AddDbConnection(
            this IServiceCollection services
         )
        {
            services.AddDbContext<CodeflixCatalogDbContext>(opt
                => opt.UseInMemoryDatabase(
                    "InMemory-DSV-Database"
                )
            );
            return services;
        }
    }
}
