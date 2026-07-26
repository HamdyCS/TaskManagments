using ErrorOr;
using MediatR;

namespace Application.Features.TaskAttachments.Commands.DeleteAttachment
{
    public sealed record DeleteAttachmentCommand(
        long WorkSpaceId,
        long ProjectId,
        long TaskId,
        long AttachmentId,
        string UserId) : IRequest<ErrorOr<Deleted>>;
}
