namespace Application.Common.Interfaces.Services
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(Stream stream, string extension, CancellationToken ct = default);
        Task DeleteFileAsync(string relativeUrl, CancellationToken ct = default);
    }
}
