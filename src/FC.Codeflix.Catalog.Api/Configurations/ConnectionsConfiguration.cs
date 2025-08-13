using FC.Codeflix.Catalog.Infra.Data.EF;
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

        public static WebApplication MigrateDatabase(
    this WebApplication app)
        {
            var environment = Environment
                .GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (environment == "EndToEndTest") return app;
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider
                .GetRequiredService<CodeflixCatalogDbContext>();
            dbContext.Database.Migrate();
            return app;
        }
    }
}
