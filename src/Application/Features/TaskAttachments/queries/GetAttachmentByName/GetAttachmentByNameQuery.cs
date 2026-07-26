using ErrorOr;
using MediatR;

namespace Application.Features.TaskAttachments.Queries.GetAttachmentByName
{
    public sealed record GetAttachmentByNameQuery(
        long WorkSpaceId,
        long ProjectId,
        long TaskId,
        string Name) : IRequest<ErrorOr<TaskAttachmentDto>>;
}
