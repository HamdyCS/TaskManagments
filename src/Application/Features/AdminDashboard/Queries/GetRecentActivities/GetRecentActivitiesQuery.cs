using Application.Common.Dtos;
using Application.Common.Dtos.AdminDashboard;
using ErrorOr;
using MediatR;

namespace Application.Features.AdminDashboard.Queries.GetRecentActivities
{
    public sealed record GetRecentActivitiesQuery(PaginationRequestDto PaginationRequestDto) : IRequest<ErrorOr<PaginationResultDto<RecentActivityDto>>>;
}
