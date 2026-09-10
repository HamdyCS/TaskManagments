using Application.Common.Dtos;
using Application.Common.Dtos.WorkSpace;


namespace Application.Features.WorkSpaces.Queries.GetWorkSpaceOverviews
{
    public sealed record GetAllWorkSpaceOverviewsQuery(
        PaginationRequestDto PaginationRequestDto,
        string? OwnerNameQuery,
        string? WorkSpaceNameQuery) : IRequest<ErrorOr<PaginationResultDto<WorkSpaceOverviewDto>>>;
}