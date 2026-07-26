using ErrorOr;
using MediatR;

namespace Application.Features.Tasks.Commands.ChangeTaskStatusByAssignedToId
{
    public sealed record ChangeTaskStatusCommandByAssignedToId(ChangeTaskStatusDto ChangeTaskStatusDto, long WorkSpaceId, long ProjectId, long TaskId, string AssignedToId) : IRequest<ErrorOr<TaskDto>>;
}
