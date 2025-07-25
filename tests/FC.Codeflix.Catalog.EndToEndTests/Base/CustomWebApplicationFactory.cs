﻿using FC.Codeflix.Catalog.Infra.Data.EF.Configurations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace FC.Codeflix.Catalog.EndToEndTests.Base
{
    public class CustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        protected override void ConfigureWebHost(
                IWebHostBuilder builder
            )
        {
            builder.UseEnvironment("EndToEndTest");
            builder.ConfigureServices(services =>
            {
                var serviceProvider = services.BuildServiceProvider();
                using (var scope = serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider
                        .GetService<CodeflixCatalogDbContext>();
                    ArgumentNullException.ThrowIfNull(context);
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();
                }
            });

            base.ConfigureWebHost(builder);
        }
    }
}
