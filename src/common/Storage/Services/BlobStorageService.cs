namespace AsteriskDotHMG.Storage.Services;

public class BlobStorageService : IBlobStorageService
{
    private readonly StorageOptions _storageOptions;

    public BlobStorageService(IOptions<StorageOptions> storageOptions)
    {
        _storageOptions = storageOptions.Value;
    }

    public async Task<string> DownloadContentAsync(SM.BlobInformation blobInfo, CancellationToken cancellationToken)
    {
        try
        {
            BlobClient blob = GetBlobClient(blobInfo);
            Response<AM.BlobDownloadResult> download = await blob.DownloadContentAsync(cancellationToken);
            string result = download.Value.Content.ToString();
            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Stream> OpenReadAsync(SM.BlobInformation blobInfo, CancellationToken cancellationToken)
    {
        try
        {
            BlobClient blob = GetBlobClient(blobInfo);
            Stream stream = await blob.OpenReadAsync(cancellationToken: cancellationToken);
            return stream;
        }
        catch (Exception)
        {
            throw;
        };
    }

    public async Task<string> UploadFileAsync(SM.BlobInformation blobInfo, Stream file, BlobUploadOptions options, CancellationToken cancellationToken)
    {
        string fileUrl = string.Empty;

        try
        {
            BlobClient blob = GetBlobClient(blobInfo);
            await blob.DeleteIfExistsAsync(cancellationToken: cancellationToken);

            if (file.Position != 0)
            {
                file.Position = 0;
            }

            Response<BlobContentInfo> response = await blob.UploadAsync(file, options, cancellationToken);
            bool isSuccess = response.GetRawResponse().Status.Equals((int)HttpStatusCode.Created);

            if (isSuccess)
            {
                fileUrl = blob.Uri.LocalPath;
            }

            return fileUrl;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<string> UploadFileAsync(SM.BlobInformation blobInfo, Stream file, CancellationToken cancellationToken)
    {
        return await UploadFileAsync(blobInfo, file, new BlobUploadOptions(), cancellationToken);
    }

    public async Task<bool> DeleteFileAsync(BlobInformation blobInfo, CancellationToken cancellationToken)
    {
        try
        {
            BlobClient blob = GetBlobClient(blobInfo);
            Azure.Response response = await blob.DeleteAsync(cancellationToken: cancellationToken);
            bool isSuccess = response.Status.Equals((int)HttpStatusCode.Accepted);
            return isSuccess;
        }
        catch (Exception)
        {
            throw;
        }
    }

    #region Private Methods
    private BlobClient GetBlobClient(SM.BlobInformation blobInfo)
    {
        try
        {
            BlobServiceClient blobServiceClient = GetServiceClient();

            BlobContainerClient container = blobServiceClient.GetBlobContainerClient(blobInfo.ContainerName);

            BlobClient blob = container.GetBlobClient(blobInfo.BlobPath);
            return blob;
        }
        catch (Exception)
        {
            throw;
        }
    }

    private BlobServiceClient GetServiceClient()
    {
        return new(_storageOptions.ConnectionString);
    }
    #endregion Private Methods
}
