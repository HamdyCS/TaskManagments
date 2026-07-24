using ErrorOr;
using MediatR;

namespace Application.Features.Tasks.Commands.RemoveAssignment
{
    public sealed record RemoveAssignmentCommand(long WorkSpaceId, long ProjectId, long TaskId, string AssignedUserId, string UserId) : IRequest<ErrorOr<bool>>;
}
