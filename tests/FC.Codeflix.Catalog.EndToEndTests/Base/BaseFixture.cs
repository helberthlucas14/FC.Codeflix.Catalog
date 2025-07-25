﻿using Bogus;
using FC.Codeflix.Catalog.Infra.Data.EF;
using FC.Codeflix.Catalog.Infra.Data.EF.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FC.Codeflix.Catalog.EndToEndTests.Base
{
    public class BaseFixture
    {
        protected Faker Faker { get; set; }

        public CustomWebApplicationFactory<Program> WebApplicationFactory { get; set; }
        public HttpClient HttpClient { get; set; }
        public ApiClient ApiClient { get; set; }
        private readonly string _dbConnectionString;
        public BaseFixture()
        {
            Faker = new Faker("pt_BR");
            WebApplicationFactory = new CustomWebApplicationFactory<Program>();
            HttpClient = WebApplicationFactory.CreateClient();
            ApiClient = new ApiClient(HttpClient);
            var configuration = WebApplicationFactory.Services
                 .GetService(typeof(IConfiguration));
            ArgumentNullException.ThrowIfNull(configuration);
            _dbConnectionString = ((IConfiguration)configuration).GetConnectionString("CatalogDb");
        }

        public CodeflixCatalogDbContext CreateDbContext()
        {

            var context = new CodeflixCatalogDbContext
                      (
                          new DbContextOptionsBuilder<CodeflixCatalogDbContext>()
                              .UseMySql(
                              _dbConnectionString,
                              ServerVersion.AutoDetect(_dbConnectionString)
                              )
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
