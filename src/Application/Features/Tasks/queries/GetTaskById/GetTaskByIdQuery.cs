using ErrorOr;
using MediatR;

namespace Application.Features.Tasks.Queries.GetTaskById
{
    public sealed record GetTaskByIdQuery(long WorkSpaceId, long ProjectId, long TaskId) : IRequest<ErrorOr<TaskDto>>;
}
