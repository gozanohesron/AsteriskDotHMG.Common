using System.Threading;

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

            if (!blobInfo.ExcludeDelete)
            {
                await blob.DeleteIfExistsAsync(cancellationToken: cancellationToken);
            }

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

            return skipIfNotExists;

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

    public async Task CopyAsync(SM.BlobInformation sourceInfo, SM.BlobInformation targetInfo, CancellationToken cancellationToken = default)
    {
        try
        {
            BlobClient srcBlob = GetBlobClient(sourceInfo);
            BlobClient destBlob = GetBlobClient(targetInfo);

            if (await srcBlob.ExistsAsync(cancellationToken))
            {
                await destBlob.StartCopyFromUriAsync(srcBlob.Uri, cancellationToken: cancellationToken);
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    #region Private Methods
    private BlobContainerClient GetContainerClient(string connectionString, BlobConnectionType connectionType, string containerName)
    {
        try
        {
            SM.BlobInformation blobInfo = new()
            {
                ConnectionString = connectionString,
                BlobConnectionType = connectionType,
                ContainerName = containerName
            };

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
            return container;
        }
        catch (Exception)
        {
            throw;
        }
    }


    private BlobClient GetBlobClient(SM.BlobInformation blobInfo)
    {
        try
        {
            BlobContainerClient container = GetContainerClient(blobInfo.ConnectionString, blobInfo.BlobConnectionType, blobInfo.ContainerName);

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

    public Uri GenerateBlobSharedAccessToken(BlobInformation blobInfo, BlobSasBuilder sasBuilder)
    {
        try
        {
            BlobClient blob = GetBlobClient(blobInfo);
            return blob.GenerateSasUri(sasBuilder);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public Uri GenerateBlobSharedAccessToken(BlobInformation blobInfo, int durationInMinutes, List<BlobSasPermissions> permissions)
    {
        try
        {
            BlobClient blob = GetBlobClient(blobInfo);

            BlobSasBuilder sasBuilder = new()
            {
                BlobContainerName = blobInfo.ContainerName,
                StartsOn = DateTimeOffset.UtcNow.AddMinutes(-10),
                ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(durationInMinutes)
            };

            foreach (BlobSasPermissions permission in permissions)
            {
                sasBuilder.SetPermissions(permission);
            }

            sasBuilder.Resource = "b";
            sasBuilder.BlobName = blobInfo.BlobPath;

            return GenerateBlobSharedAccessToken(blobInfo, sasBuilder);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> ExistsAsync(BlobInformation blobInfo, CancellationToken cancellationToken = default)
    {
        try
        {
            BlobClient blob = GetBlobClient(blobInfo);
            return await blob.ExistsAsync(cancellationToken);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> DeleteDirectoryAsync(BlobDirectoryInformation blobDirectoryInfo, CancellationToken cancellationToken = default)
    {
        try
        {
            BlobContainerClient containerClient = GetContainerClient(blobDirectoryInfo.ConnectionString, blobDirectoryInfo.BlobConnectionType, blobDirectoryInfo.ContainerName);

            await foreach (var blobItem in containerClient.GetBlobsAsync(prefix: blobDirectoryInfo.Directory, cancellationToken: cancellationToken))
            {
                // Create a BlobClient for each blob
                BlobClient blobClient = containerClient.GetBlobClient(blobItem.Name);

                // Delete the blob
                await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
            }

            return true;
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion Private Methods
}
