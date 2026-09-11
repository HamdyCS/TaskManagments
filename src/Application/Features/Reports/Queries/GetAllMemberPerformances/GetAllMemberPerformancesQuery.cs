using Application.Common.Dtos;
using Domain.Common.Pagination;
using ErrorOr;
using MediatR;

namespace Application.Features.Reports.Queries.GetAllMemberPerformances
{
    public sealed record GetAllMemberPerformancesQuery(
        PaginationRequestDto PaginationRequestDto,
        string? MemberNameQuery) : IRequest<ErrorOr<PaginationResult<MemberPerformanceDto>>>;
}
