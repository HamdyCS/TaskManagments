using ErrorOr;
using MediatR;

namespace Application.Features.Tasks.Commands.DeleteTask
{
    public sealed record DeleteTaskCommand(long WorkSpaceId, long ProjectId, long TaskId, string UserId) : IRequest<ErrorOr<bool>>;
}
