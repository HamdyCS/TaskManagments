using ErrorOr;
using MediatR;

namespace Application.Features.Tasks.Commands.CreateTask
{
    public sealed record CreateTaskCommand(CreateTaskDto CreateTaskDto, long WorkSpaceId, long ProjectId, string UserId) : IRequest<ErrorOr<TaskDto>>;
}
