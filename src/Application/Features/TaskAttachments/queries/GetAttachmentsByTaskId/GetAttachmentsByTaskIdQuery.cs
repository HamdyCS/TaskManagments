using ErrorOr;
using MediatR;

namespace Application.Features.TaskAttachments.Queries.GetAttachmentsByTaskId
{
    public sealed record GetAttachmentsByTaskIdQuery(
        long WorkSpaceId,
        long ProjectId,
        long TaskId) : IRequest<ErrorOr<List<TaskAttachmentDto>>>;
}
