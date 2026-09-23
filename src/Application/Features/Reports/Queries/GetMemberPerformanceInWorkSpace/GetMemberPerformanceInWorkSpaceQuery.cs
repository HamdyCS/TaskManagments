using Application.Common.Dtos;
using ErrorOr;
using MediatR;

namespace Application.Features.Reports.Queries.GetMemberPerformanceInWorkSpace
{
    public sealed record GetMemberPerformanceInWorkSpaceQuery(string userId,long WorkspaceId) : IRequest<ErrorOr<MemberPerformanceDto>>;
}
