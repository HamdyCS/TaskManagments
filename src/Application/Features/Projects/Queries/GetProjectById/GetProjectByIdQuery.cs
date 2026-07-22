using ErrorOr;
using MediatR;

namespace Application.Features.Projects.Queries.GetProjectById
{
    public sealed record GetProjectByIdQuery(long WorkSpaceId, long ProjectId) : IRequest<ErrorOr<ProjectDto>>;
}
