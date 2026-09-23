using Application.Common.Dtos;
using ErrorOr;
using MediatR;

namespace Application.Features.Reports.Queries.GetMemberPerformancesInWorkSpace
{
    public sealed record GetMemberPerformancesInWorkSpaceQuery(long WorkspaceId) : IRequest<ErrorOr<MemberPerformanceDto>>;
}
