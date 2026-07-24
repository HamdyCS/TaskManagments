using ErrorOr;
using MediatR;

namespace Application.Features.Tasks.Commands.ChangeTaskStatus
{
    public sealed record ChangeTaskStatusCommand(ChangeTaskStatusDto ChangeTaskStatusDto, long WorkSpaceId, long ProjectId, long TaskId, string UserId) : IRequest<ErrorOr<TaskDto>>;
}
