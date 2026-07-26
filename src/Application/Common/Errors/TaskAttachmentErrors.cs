using ErrorOr;

namespace Application.Common.Errors
{
    public static class TaskAttachmentErrors
    {
        public static Error NotFound(long attachmentId)
            => Error.NotFound("Attachment_NotFound", $"Attachment not found with id {attachmentId}");

        public static Error NotFoundByName(string name)
            => Error.NotFound("Attachment_NotFoundByName", $"Attachment not found with name '{name}'");

        public static Error TaskNotFound(long taskId)
            => Error.NotFound("Attachment_TaskNotFound", $"Task not found with id {taskId}");

        public static Error ProjectNotFound(long projectId)
            => Error.NotFound("Attachment_ProjectNotFound", $"Project not found with id {projectId}");

        public static Error EmptyFile()
            => Error.Validation("Attachment_EmptyFile", "File is empty.");

        public static Error FileTooLarge()
            => Error.Validation("Attachment_FileTooLarge", "File exceeds 50 MB limit.");

        public static Error InvalidExtension()
            => Error.Validation("Attachment_InvalidExtension", "File extension is not allowed.");

        public static Error InvalidMimeType()
            => Error.Validation("Attachment_InvalidMimeType", "File MIME type is not allowed.");

        public static Error UnauthorizedAccess()
            => Error.Forbidden("Attachment_UnauthorizedAccess", "You are not authorized to perform this action on this attachment.");

        public static Error FileSaveFailed()
            => Error.Failure("Attachment_FileSaveFailed", "Physical file could not be saved.");

        public static Error FileDeleteFailed()
            => Error.Failure("Attachment_FileDeleteFailed", "Physical file could not be deleted.");

        public static Error DatabaseSaveFailed()
            => Error.Failure("Attachment_DatabaseSaveFailed", "Database record could not be saved.");
    }
}
