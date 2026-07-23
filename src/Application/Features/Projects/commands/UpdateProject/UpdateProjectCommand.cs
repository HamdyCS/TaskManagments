using ErrorOr;
using MediatR;

namespace Application.Features.Projects.Commands.UpdateProject
{
    public sealed record UpdateProjectCommand(UpdateProjectDto UpdateProjectDto, long WorkSpaceId, long ProjectId, string UserId) : IRequest<ErrorOr<Success>>;
}
