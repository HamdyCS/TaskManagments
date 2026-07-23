using ErrorOr;
using MediatR;

namespace Application.Features.Projects.Commands.CreateProject
{
    public sealed record CreateProjectCommand(CreateProjectDto CreateProjectDto, long WorkSpaceId, string UserId) : IRequest<ErrorOr<ProjectDto>>;
}
