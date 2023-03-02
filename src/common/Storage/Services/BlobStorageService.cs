using Microsoft.Extensions.Options;

namespace AsteriskDotHMG.Storage.Services;

public class BlobStorageService : IBlobStorageService
{
    private static BlobServiceClient _blobServiceClient;
    private readonly BlobStorageOptions _blobStorageOptions;

    public BlobStorageService(IOptions<BlobStorageOptions> blobStorageOptions)
    {
        _blobStorageOptions = blobStorageOptions.Value;
    }

    public async Task<string> DownloadContentAsync(SM.BlobInformation blobInfo, CancellationToken cancellationToken)
    {
        try
        {
            BlobClient blob = GetBlob(blobInfo);
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
            BlobClient blob = GetBlob(blobInfo);
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
            BlobClient blob = GetBlob(blobInfo);
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
            BlobClient blob = GetBlob(blobInfo);
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
    private BlobClient GetBlob(SM.BlobInformation blobInfo)
    {
        try
        {
            InitClient();

            BlobContainerClient container = _blobServiceClient.GetBlobContainerClient(blobInfo.ContainerName);

            BlobClient blob = container.GetBlobClient(blobInfo.BlobPath);
            return blob;
        }
        catch (Exception)
        {
            throw;
        }
    }

    private void InitClient()
    {
        _blobServiceClient = new(_blobStorageOptions.ConnectionString);
    }
    #endregion Private Methods
}
