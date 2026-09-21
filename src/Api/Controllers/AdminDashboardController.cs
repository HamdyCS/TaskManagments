using Application.Common.Dtos;
using Application.Features.AdminDashboard.Queries.GetAdminDashboard;
using Application.Features.AdminDashboard.Queries.GetRecentActivities;
using Api.Polices;
using Domain.Common.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Api.Controllers
{
    [Route("api/admin/dashboard")]
    [ApiController]
    [Authorize(Roles = nameof(Role.Admin))]
    [EnableRateLimiting(nameof(RateLimitingPolicies.DashboardRateLimit))]
    public class AdminDashboardController(IMediator mediator) : ControllerBase
    {
        [HttpGet(Name = "GetAdminDashboard")]
        public async Task<IActionResult> GetAdminDashboard()
        {
            var userId = User.GetUserId();
            if (userId is null)
            {
                return Unauthorized();
            }

            var result = await mediator.Send(new GetAdminDashboardQuery(userId));

            return result.Match(
                dashboard => Ok(dashboard),
                errors => errors.ToProblemDetailsObjectResult());
        }

        [HttpGet("recent-activities", Name = "GetRecentActivities")]
        public async Task<IActionResult> GetRecentActivities([FromQuery] PaginationRequestDto paginationRequest)
        {
            var result = await mediator.Send(new GetRecentActivitiesQuery(paginationRequest));

            return result.Match(
                activities => Ok(activities),
                errors => errors.ToProblemDetailsObjectResult());
        }
    }
}
