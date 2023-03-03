namespace AsteriskDotHMG.Storage.Models;

public class StorageOptions
{
    public string ConnectionString { get; set; }

    public string BlobEndpoint
    {
        get
        {
            return GetEndpoint("blob");
        }
    }

    public string QueueEndpoint
    {
        get
        {
            return GetEndpoint("queue");
        }
    }

    public string TableEndpoint
    {
        get
        {
            return GetEndpoint("table");
        }
    }

    private string GetEndpoint(string name)
    {
        string type = $"{name.ToTitleCase()}Endpoint";
        string endpoint = GetValueFromConnectionString(type);

        if (!string.IsNullOrEmpty(endpoint))
        {
            return endpoint;
        }

        string protocol = GetValueFromConnectionString("DefaultEndpointsProtocol");
        string accountName = GetValueFromConnectionString("AccountName");
        string endpointSuffix = GetValueFromConnectionString("EndpointSuffix");

        if (!string.IsNullOrEmpty(protocol) && !string.IsNullOrEmpty(accountName) && !string.IsNullOrEmpty(endpointSuffix))
        {
            return $"{protocol}://{accountName}.{name}.{endpointSuffix}";
        }
       
        return string.Empty;
    }

    private string GetValueFromConnectionString(string key)
    {
        if (!string.IsNullOrEmpty(ConnectionString) && !string.IsNullOrEmpty(key))
        {
            string[] connectionNames = ConnectionString.Split(';');

            if (connectionNames.Length > 0)
            {
                string value = connectionNames.Where(e => e.StartsWith(key)).FirstOrDefault();

                if (!string.IsNullOrEmpty(value))
                {
                    return value.Replace($"{key}=", string.Empty);
                }
            }
        }

        return string.Empty;
    }
}