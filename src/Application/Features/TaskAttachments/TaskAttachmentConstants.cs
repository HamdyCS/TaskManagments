namespace Application.Features.TaskAttachments
{
    public static class TaskAttachmentConstants
    {
        public static readonly HashSet<string> AllowedExtensions =
            new(StringComparer.OrdinalIgnoreCase) { ".pdf", ".jpg", ".jpeg", ".png" };

        public static readonly HashSet<string> AllowedMimeTypes =
            new(StringComparer.OrdinalIgnoreCase) { "application/pdf", "image/jpeg", "image/png" };

        public const long MaxFileSize = 50L * 1024 * 1024; // 50 MB
    }
}
