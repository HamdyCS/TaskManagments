using Microsoft.AspNetCore.Http;

namespace Application.Features.TaskAttachments.Commands.UploadAttachment
{
    public sealed record UploadAttachmentDto(IFormFile File);
}
