using ErrorOr;
using MediatR;

namespace Application.Features.Tasks.Commands.UpdateTask
{
    public sealed record UpdateTaskCommand(UpdateTaskDto UpdateTaskDto, long WorkSpaceId, long ProjectId, long TaskId, string UserId) : IRequest<ErrorOr<TaskDto>>;
}
