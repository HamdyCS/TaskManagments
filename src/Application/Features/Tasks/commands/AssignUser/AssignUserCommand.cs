using ErrorOr;
using MediatR;

namespace Application.Features.Tasks.Commands.AssignUsers
{
    public sealed record AssignUserCommand(AssignUsersDto AssignUserDto, long WorkSpaceId, long ProjectId, long TaskId, string UserId) : IRequest<ErrorOr<TaskAssignmentDto>>;
}
