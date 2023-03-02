namespace AsteriskDotHMG.Storage.Models;

public class BlobInformation
{
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

            return string.Empty;
        }
    }
}
