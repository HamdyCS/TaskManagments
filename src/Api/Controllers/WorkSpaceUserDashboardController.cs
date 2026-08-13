using Application.Features.WorkSpaceUserDashboard.Queries.GetWorkSpaceUserDashboard;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/workspaces/{workspaceId}/dashboard")]
    [ApiController]
    public class WorkSpaceUserDashboardController(IAuthorizationService authorizationService
        , IMediator mediator) : ControllerBase
    {
        public async Task<bool> IsOwnerOrProjectManagerAsync(long workspaceId)
        {

            //Check if use is owner
            var isOwnerAuthResult = await authorizationService.
                AuthorizeAsync(User, workspaceId, "WorkSpaceOwner");
            if (isOwnerAuthResult.Succeeded)
                return true;


            //Check if user is WorkSpaceProjectManager
            var isProductManagerAuthResult = await authorizationService.
                AuthorizeAsync(User, workspaceId, "WorkSpaceProjectManager");
            if (isProductManagerAuthResult.Succeeded)
                return true;

            return false;
        }

        public async Task<bool> IsWorkSpaceUser(long workspaceId)
        {
            //Check if use is owner
            var isWorkSpaceUserResult = await authorizationService.
                AuthorizeAsync(User, workspaceId, "WorkSpaceUser");

            return isWorkSpaceUserResult.Succeeded;
        }
        [HttpGet]
        public async Task<IActionResult> GetWorkSpaceUserDashboard([FromRoute] long workspaceId)
        {
            //get userId from claims
            var userId = User.GetUserId();
            if(userId is null)
            {
                return Unauthorized();
            }

            //get user role in workspace
            var isOwnerOrProjectManager = await IsOwnerOrProjectManagerAsync(workspaceId);
            if(!isOwnerOrProjectManager)
            {
                var isWorkSpaceUser = await IsWorkSpaceUser(workspaceId);
                if (!isWorkSpaceUser)
                {
                    return Forbid();
                }
            }

            var result = await mediator.
                Send(new GetWorkSpaceUserDashboardQuery(userId, workspaceId, 
                !isOwnerOrProjectManager));
          
            return result.Match(
                dashboard => Ok(dashboard),
                errors => errors.ToProblemDetailsObjectResult());
        }
    }
}

