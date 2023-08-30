namespace AsteriskDotHMG.Storage.Models;

public class BlobInformation
{
    public BlobInformation()
    {

    }

    public BlobInformation(string containerName, string directory, string fileName)
    {
        ContainerName = containerName;
        Directory = directory;
        FileName = fileName;
    }

    public string ContainerName { get; set; }

    public string Directory { get; set; }

    public string FileName { get; set; }

    public string BlobPath
    {
        get
        {
            if (!string.IsNullOrEmpty(Directory) && !string.IsNullOrEmpty(FileName))
            {
                return $"{Directory}/{FileName}";
            }
            else  if (!string.IsNullOrEmpty(FileName))
            {
                return FileName;
            }

            return string.Empty;
        }
    }
}
