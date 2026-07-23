using Application.Common.Dtos;
using ErrorOr;
using MediatR;

namespace Application.Features.Projects.Queries.GetAllProjects
{
    public sealed record GetAllProjectsQuery(long WorkSpaceId, PaginationRequestDto PaginationRequest) : IRequest<ErrorOr<PaginationResultDto<ProjectDto>>>;
}
