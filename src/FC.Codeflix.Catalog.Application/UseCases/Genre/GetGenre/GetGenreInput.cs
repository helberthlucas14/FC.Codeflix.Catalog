using FC.Codeflix.Catalog.Application.UseCases.Genre.Common;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Genre.GetGenre
{
    public interface IGetGenre
    : IRequestHandler<GetGenreInput, GenreModelOutput>
    { }
    public class GetGenreInput
        : IRequest<GenreModelOutput>
    {
        public GetGenreInput(Guid id)
            => Id = id;

        public Guid Id { get; set; }
    }
}
