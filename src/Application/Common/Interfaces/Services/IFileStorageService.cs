namespace Application.Common.Interfaces.Services
{
    public interface IFileStorageService
    {
        Task<string> SaveTaskAttachmentFileAsync(Stream stream, string extension, CancellationToken ct = default);
        Task DeleteTaskAttachmentFileAsync(string relativeUrl, CancellationToken ct = default);
        Task<Stream> GetTaskAttachmentFileAsync(string relativePath, CancellationToken ct = default);
    }
}
