namespace AsteriskDotHMG.Storage.Interfaces;

public interface IBlobStorageService
{
    Task<string> DownloadContentAsync(SM.BlobInformation blobInfo, CancellationToken cancellationToken);
    Task<Stream> OpenReadAsync(SM.BlobInformation blobInfo, CancellationToken cancellationToken);
    Task<string> UploadFileAsync(SM.BlobInformation blobInfo, Stream file, BlobUploadOptions options, CancellationToken cancellationToken);
    Task<string> UploadFileAsync(SM.BlobInformation blobInfo, Stream file, CancellationToken cancellationToken);
    Task<bool> DeleteFileAsync(SM.BlobInformation blobInfo, CancellationToken cancellationToken);
}
