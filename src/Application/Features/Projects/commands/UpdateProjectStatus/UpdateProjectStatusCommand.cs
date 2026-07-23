using ErrorOr;
using MediatR;

namespace Application.Features.Projects.Commands.UpdateProjectStatus
{
    public sealed record UpdateProjectStatusCommand(
        UpdateProjectStatusDto UpdateProjectStatusDto,
        long WorkSpaceId,
        long ProjectId,
        string UserId
    ) : IRequest<ErrorOr<Success>>;
}
