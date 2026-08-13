using Application.Common.Dtos;
using ErrorOr;
using MediatR;

namespace Application.Features.Reports.Queries.GetMemberPerformanceInWorkSpace
{
    public sealed record GetMemberPerformanceInWorkSpaceQuery(long WorkspaceId, string MemberId) : IRequest<ErrorOr<MemberPerformanceDto>>;
}
