using Application.Common.Dtos;
using ErrorOr;
using MediatR;

namespace Application.Features.Reports.Queries.GetMemberPerformanceInWorkSpace
{
    public sealed record GetMemberPerformanceInWorkSpaceQuery(long WorkspaceId) : IRequest<ErrorOr<MemberPerformanceDto>>;
}
