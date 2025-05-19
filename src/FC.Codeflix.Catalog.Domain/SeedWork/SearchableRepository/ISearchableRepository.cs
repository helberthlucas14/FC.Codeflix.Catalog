namespace FC.Codeflix.Catalog.Domain.SeedWork.SearchableRepository
{
    public interface ISearchableRepository<TAggregate> 
        where TAggregate : AggregateRoot
    {
        Task<SeachOutput<TAggregate>> Search(
            SeachInput input,
            CancellationToken cancellationToken
            );
    }
}
