﻿using FC.Codeflix.Catalog.Application.Exceptions;
using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Repository;
using FC.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using FC.Codeflix.Catalog.Infra.Data.EF.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FC.Codeflix.Catalog.Infra.Data.EF.Repositories
{
    public class CategoryRepository
        : ICategoryRepository
    {
        private readonly CodeflixCatalogDbContext _context;
        private DbSet<Category> _categories => _context.Set<Category>();

        public CategoryRepository(
            CodeflixCatalogDbContext context
            )
        {
            _context = context;
        }

        public async Task Delete(Category aggregate, CancellationToken _)
            => await Task.FromResult(_categories.Remove(aggregate));

        public async Task<Category> Get(Guid id, CancellationToken cancellationToken)
        {
            var category = await _categories.AsNoTracking()
                .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken
             );

            NotFoundException.ThrowIfNull(category, $"Category '{id}' not found.");
            return category!;
        }


        public async Task Insert(Category aggregate, CancellationToken cancellationToken)
            => await _context.AddAsync(aggregate, cancellationToken);

        public async Task<SearchOutput<Category>> Search(SearchInput input, CancellationToken cancellationToken)
        {
            var toSkip = (input.Page - 1) * input.PerPage;
            var query = _categories.AsNoTracking();
            query = AddOrderToQuery(query, input.OrderBy, input.Order);

            if (!string.IsNullOrWhiteSpace(input.Search))
                query = query.Where(x => x.Name.Contains(input.Search));

            var total = await query.CountAsync();
            var items = await query
                .Skip(toSkip)
                .Take(input.PerPage)
                .ToListAsync();

            return new(input.Page, input.PerPage, total, items);
        }

        private IQueryable<Category> AddOrderToQuery(
            IQueryable<Category> query,
            string orderProperty,
            SearchOrder order
            )
        {
            var orderedQuery = (orderProperty.ToLower(), order) switch
            {
                ("name", SearchOrder.Asc) => query.OrderBy(x => x.Name),
                ("name", SearchOrder.Desc) => query.OrderByDescending(x => x.Name),
                ("id", SearchOrder.Asc) => query.OrderBy(x => x.Id),
                ("id", SearchOrder.Desc) => query.OrderByDescending(x => x.Id),
                ("createdAt", SearchOrder.Asc) => query.OrderBy(x => x.CreatedAt),
                ("createdAt", SearchOrder.Desc) => query.OrderByDescending(x => x.CreatedAt),
                _ => query.OrderBy(x => x.Name)
            };

            return orderedQuery.ThenBy(x => x.CreatedAt);
        }

        public async Task<IReadOnlyList<Guid>> GetIdsListByIds(
            List<Guid> ids,
            CancellationToken cancellationToken
       )
                 => await _categories.AsNoTracking()
                        .Where(category => ids.Contains(category.Id))
                        .Select(category => category.Id).ToListAsync();

        public async Task<IReadOnlyList<Category>> GetListByIds(
            List<Guid> ids,
            CancellationToken cancellationToken
            )
            => await _categories.AsNoTracking()
                .Where(category => ids.Contains(category.Id))
                .ToListAsync();


        public Task Update(Category aggregate, CancellationToken _)
            => Task.FromResult(_categories.Update(aggregate));
    }
}
