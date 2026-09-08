using Application.Common.Dtos.AdminDashboard;
using ErrorOr;
using MediatR;

namespace Application.Features.AdminDashboard.Queries.GetAdminDashboard
{
    public sealed record GetAdminDashboardQuery(string UserId) : IRequest<ErrorOr<AdminDashboardDto>>;
}
