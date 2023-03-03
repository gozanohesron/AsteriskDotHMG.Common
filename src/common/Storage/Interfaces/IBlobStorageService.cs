namespace AsteriskDotHMG.Storage.Interfaces;

public interface IBlobStorageService
{
    Task<string> DownloadContentAsync(SM.BlobInformation blobInfo, CancellationToken cancellationToken = default);
    Task<Stream> OpenReadAsync(SM.BlobInformation blobInfo, CancellationToken cancellationToken = default);
    Task<string> UploadFileAsync(SM.BlobInformation blobInfo, Stream file, BlobUploadOptions options, CancellationToken cancellationToken = default);
    Task<string> UploadFileAsync(SM.BlobInformation blobInfo, Stream file, CancellationToken cancellationToken = default);
    Task<bool> DeleteFileAsync(SM.BlobInformation blobInfo, CancellationToken cancellationToken = default);
    Task<bool> DeleteFileAsync(SM.BlobInformation blobInfo, bool skipIfNotExists = false, CancellationToken cancellationToken = default);
}
