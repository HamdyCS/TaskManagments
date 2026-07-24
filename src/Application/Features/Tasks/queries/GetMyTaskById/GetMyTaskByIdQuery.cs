using ErrorOr;
using MediatR;

namespace Application.Features.Tasks.Queries.GetMyTaskById
{
    public sealed record GetMyTaskByIdQuery(long WorkSpaceId, long ProjectId, long TaskId, string AssignedToId) : IRequest<ErrorOr<TaskDto>>;
}
