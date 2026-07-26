using ErrorOr;
using MediatR;

namespace Application.Features.TaskAttachments.Queries.GetAttachmentById
{
    public sealed record GetAttachmentByIdQuery(
        long WorkSpaceId,
        long ProjectId,
        long TaskId,
        long AttachmentId) : IRequest<ErrorOr<TaskAttachmentDto>>;
}
