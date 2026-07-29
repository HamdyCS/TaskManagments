using Application.Common.Dtos;
using ErrorOr;
using MediatR;

namespace Application.Features.Reports.Queries.GetMemberPerformanceInProject
{
    public sealed record GetMemberPerformanceInProjectQuery(long ProjectId, string MemberId) : IRequest<ErrorOr<MemberPerformance>>;
}
