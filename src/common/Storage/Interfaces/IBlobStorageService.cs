using Azure.Storage.Sas;
using System.Threading.Tasks;

namespace AsteriskDotHMG.Storage.Interfaces;

public interface IBlobStorageService
{
    Task<string> DownloadContentAsync(SM.BlobInformation blobInfo, CancellationToken cancellationToken = default);
    Task<Stream> DownloadStreamAsync(SM.BlobInformation blobInfo, CancellationToken cancellationToken = default);
    Task<byte[]> DownloadByteAsync(SM.BlobInformation blobInfo, CancellationToken cancellationToken = default);
    Task<string> UploadFileAsync(SM.BlobInformation blobInfo, Stream file, BlobUploadOptions options, CancellationToken cancellationToken = default);
    Task<string> UploadFileAsync(SM.BlobInformation blobInfo, Stream file, CancellationToken cancellationToken = default);
    Task<bool> DeleteFileAsync(SM.BlobInformation blobInfo, CancellationToken cancellationToken = default);
    Task<bool> DeleteFileAsync(SM.BlobInformation blobInfo, bool skipIfNotExists = false, CancellationToken cancellationToken = default);
    Task<string> GetBase64Data(SM.BlobInformation blobInfo, CancellationToken cancellationToken = default);
    Uri GenerateBlobSharedAccessToken(BlobInformation blobInfo, int durationInMinutes, List<BlobSasPermissions> permissions);
    Task<bool> ExistsAsync(SM.BlobInformation blobInfo, CancellationToken cancellationToken = default);
}
