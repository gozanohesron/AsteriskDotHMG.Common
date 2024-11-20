namespace AsteriskDotHMG.Storage.Models;

public class BlobDirectoryInformation
{
    public BlobDirectoryInformation()
    {

    }

    public BlobDirectoryInformation(string containerName, string directory)
    {
        ContainerName = containerName;
        Directory = directory;
    }

    public string ContainerName { get; set; }

    public string Directory { get; set; }

    public string ConnectionString { get; set; }

    public BlobConnectionType BlobConnectionType { get; set; } = BlobConnectionType.ConnectionString;
}
