using Application.Common.Dtos.Dashboard;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Dashboard.Queries.GetDashboard
{
    public sealed record GetDashboardQuery(string UserId, long WorkSpaceId,bool SpecificallyForThisUser = true) : IRequest<ErrorOr<DashboardDto>>;
}