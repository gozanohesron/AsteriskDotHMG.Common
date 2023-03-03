using Azure.Storage.Queues;
using System.Text;

namespace AsteriskDotHMG.Storage.Services;

public class QueueStorageService : IQueueStorageService
{
    private readonly StorageOptions _storageOptions;

    public QueueStorageService(IOptions<StorageOptions> storageOptions)
    {
        _storageOptions = storageOptions.Value;
    }

    public async Task<bool> AddMessageQueueAsync<T>(string queueName, T queueBody, CancellationToken cancellationToken = default)
    {
        try
        {
            QueueClient queueClient = GetQueueClient(queueName, cancellationToken);

            queueClient.CreateIfNotExists(cancellationToken: cancellationToken);

            if (queueClient.Exists(cancellationToken))
            {
                string queuePayload = JsonConvert.SerializeObject(queueBody);
                var response = await queueClient.SendMessageAsync(Convert.ToBase64String(Encoding.UTF8.GetBytes(queuePayload)), cancellationToken);

                bool isSuccess = response.GetRawResponse().Status.Equals((int)HttpStatusCode.Created);
                return isSuccess;
            }

            return false;
        }
        catch (Exception)
        {
            throw;
        }
        
    }

    #region Private Methods

    private QueueClient GetQueueClient(string queueName, CancellationToken cancellationToken = default)
    {
        QueueClient queueClient = new(_storageOptions.ConnectionString, queueName);
        queueClient.CreateIfNotExists(cancellationToken: cancellationToken);

        return queueClient;
    }
    #endregion Private Methods
}
