using ErrorOr;
using MediatR;

namespace Application.Features.TaskAttachments.Commands.UploadAttachment
{

    public sealed record UploadAttachmentCommand(
        UploadAttachmentDto UploadAttachmentDto,
        long WorkSpaceId,
        long ProjectId,
        long TaskId,
        string UserId) : IRequest<ErrorOr<TaskAttachmentDto>>;
}
