namespace FC.Codeflix.Catalog.Domain.SeedWork;

public interface IMessageProducer
{
    Task SendMessageAsync<T>(T message, CancellationToken cancellationToken);
}