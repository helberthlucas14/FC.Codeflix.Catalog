using FC.Codeflix.Catalog.Infra.Data.EF.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FC.Codeflix.Catalog.Api.Configurations
{
    public static class ConnectionsConfiguration
    {
        public static IServiceCollection AddAndConfigConnections(
            this IServiceCollection services,
            IConfiguration configuration
            )
        {
            services.AddDbConnection(configuration);
            return services;
        }

        private static IServiceCollection AddDbConnection(
            this IServiceCollection services,
            IConfiguration configuration
         )
        {
            var connectionString = configuration
                .GetConnectionString("CatalogDb");
            ArgumentNullException.ThrowIfNull(connectionString);
            services.AddDbContext<CodeflixCatalogDbContext>(opt
                => opt.UseMySql(
                    connectionString,
                    ServerVersion.AutoDetect(connectionString)
                    )
            );
            return services;
        }
    }
}
