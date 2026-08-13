using Application.Common.Dtos.WorkSpaceUserDashboard;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaceUserDashboard.Queries.GetWorkSpaceUserDashboard
{
    public sealed record GetWorkSpaceUserDashboardQuery(string UserId, long WorkSpaceId,bool SpecificallyForThisUser = true) : IRequest<ErrorOr<WorkSpaceDashboardDto>>;
}