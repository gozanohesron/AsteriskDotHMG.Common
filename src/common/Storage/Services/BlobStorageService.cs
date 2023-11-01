namespace AsteriskDotHMG.Storage.Services;

public class BlobStorageService : IBlobStorageService
{
    private readonly StorageOptions _storageOptions;

    public BlobStorageService(IOptions<StorageOptions> storageOptions)
    {
        _storageOptions = storageOptions.Value;
    }

    public async Task<string> DownloadContentAsync(SM.BlobInformation blobInfo, CancellationToken cancellationToken = default)
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

    public async Task<Stream> DownloadStreamAsync(SM.BlobInformation blobInfo, CancellationToken cancellationToken = default)
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

    public async Task<byte[]> DownloadByteAsync(SM.BlobInformation blobInfo, CancellationToken cancellationToken = default)
    {
        try
        {
            BlobClient blob = GetBlobClient(blobInfo);
            Stream stream = await blob.OpenReadAsync(cancellationToken: cancellationToken);
            MemoryStream memoryStream = new();
            stream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
        catch (Exception)
        {
            throw;
        };
    }

    public async Task<string> UploadFileAsync(SM.BlobInformation blobInfo, Stream file, BlobUploadOptions options, CancellationToken cancellationToken = default)
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

    public async Task<string> UploadFileAsync(SM.BlobInformation blobInfo, Stream file, CancellationToken cancellationToken = default)
    {
        return await UploadFileAsync(blobInfo, file, new BlobUploadOptions(), cancellationToken);
    }

    public async Task<bool> DeleteFileAsync(BlobInformation blobInfo, CancellationToken cancellationToken = default)
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

    public async Task<bool> DeleteFileAsync(SM.BlobInformation blobInfo, bool skipIfNotExists = false, CancellationToken cancellationToken = default)
    {
        try
        {
            BlobClient blob = GetBlobClient(blobInfo);

            if (await blob.ExistsAsync(cancellationToken))
            {
                return await DeleteFileAsync(blobInfo, cancellationToken);
            }

            return true;

        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<string> GetBase64Data(SM.BlobInformation blobInfo, CancellationToken cancellationToken = default)
    {
        try
        {
            Stream stream = await DownloadStreamAsync(blobInfo, cancellationToken);

            if (stream != null)
            {
                using MemoryStream memoryStream = new();
                await stream.CopyToAsync(memoryStream, cancellationToken);
                string base64 = Convert.ToBase64String(memoryStream.ToArray());
                return base64;
            }

            return string.Empty;
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
            BlobServiceClient blobServiceClient = null;

            if (!string.IsNullOrEmpty(blobInfo.ConnectionString))
            {
                if (blobInfo.BlobConnectionType == BlobConnectionType.ConnectionString)
                {
                    blobServiceClient = GetServiceClient(blobInfo.ConnectionString);
                }
                else
                {
                    blobServiceClient = GetServiceClient(new Uri(blobInfo.ConnectionString));
                }
            }
            else
            {
                blobServiceClient = GetServiceClient();
            }

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

    private static BlobServiceClient GetServiceClient(string connectionString)
    {
        return new(connectionString);
    }

    private static BlobServiceClient GetServiceClient(Uri sasUri)
    {
        return new(sasUri);
    }

    public Uri GenerateBlobSharedAccessToken(BlobInformation blobInfo, int durationInMinutes, List<BlobSasPermissions> permissions)
    {
        try
        {
            BlobClient blob = GetBlobClient(blobInfo);

            BlobSasBuilder sasBuilder = new()
            {
                BlobContainerName = blobInfo.ContainerName,
                StartsOn = DateTimeOffset.UtcNow,
                ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(durationInMinutes)
            };

            foreach (BlobSasPermissions permission in permissions)
            {
                sasBuilder.SetPermissions(permission);
            }

            sasBuilder.Resource = "b";
            sasBuilder.BlobName = blobInfo.BlobPath;

            return blob.GenerateSasUri(sasBuilder);
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion Private Methods
}
