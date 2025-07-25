﻿using FC.Codeflix.Catalog.Application.Interfaces;
using FC.Codeflix.Catalog.Infra.Data.EF.Configurations;

namespace FC.Codeflix.Catalog.Infra.Data.EF
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CodeflixCatalogDbContext _context;

        public UnitOfWork(CodeflixCatalogDbContext context)
            => _context = context;

        public Task Commit(CancellationToken cancellationToken)
            => _context.SaveChangesAsync(cancellationToken);

        public Task Rollback(CancellationToken none)
            => Task.CompletedTask;
    }
}
