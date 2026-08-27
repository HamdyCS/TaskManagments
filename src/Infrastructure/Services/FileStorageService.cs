using Application.Common.Interfaces.Services;
using Microsoft.AspNetCore.Hosting;

namespace Infrastructure.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly string _uploadsFolder;

        public FileStorageService(IWebHostEnvironment env)
        {
            _uploadsFolder = Path.Combine(env.WebRootPath, "uploads", "task-attachments");
            if (!Directory.Exists(_uploadsFolder))
                Directory.CreateDirectory(_uploadsFolder);
        }

        public async Task<string> SaveFileAsync(Stream stream, string extension, CancellationToken ct = default)
        {
            var storedFileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(_uploadsFolder, storedFileName);

            await using var fileStream = 
                new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 81920, true);
            await stream.CopyToAsync(fileStream, ct);

            return $"uploads/task-attachments/{storedFileName}";
        }

        public async Task DeleteFileAsync(string relativePath, CancellationToken ct = default)
        {
            var filePath = Path.Combine(_uploadsFolder, Path.GetFileName(relativePath));

            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }

        }

        public async Task<Stream> GetFileAsync (string relativePath, CancellationToken ct = default)
        {
            var filePath = Path.Combine(_uploadsFolder, Path.GetFileName(relativePath));

            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found");

            return new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 81920, true);
        }
    }
}
