using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Domain.Repository;
using FC.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.ListCategories
{
    public class ListCategories : IListCategories
    {
        private readonly ICategoryRepository _categoryRepository;

        public ListCategories(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<ListCategoriesOutput> Handle(ListCategoriesInput request, CancellationToken cancellationToken)
        {
            var seachOutput = await _categoryRepository.Search(new SeachInput(
                 request.Page,
                 request.PerPage,
                 request.Search,
                 request.Sort,
                 request.Dir
                 ),
                cancellationToken
             );

            return new ListCategoriesOutput(
                seachOutput.CurrentPage,
                seachOutput.PerPage,
                seachOutput.Total,
                seachOutput.Items
                .Select(CategoryModelOutput.FromCategory)
                .ToList()
              );
        }
    }
}
