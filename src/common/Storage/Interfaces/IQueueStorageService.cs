namespace AsteriskDotHMG.Storage.Interfaces;

public interface IQueueStorageService
{
    Task<bool> AddMessageQueueAsync<T>(string queueName, T queueBody, CancellationToken cancellationToken);
}
