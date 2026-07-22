using Application.Common.Interfaces.Services;

namespace Api.Polices.WorkSpace.WorkSpaceProjectManager
{
    public class WorkSpaceProjectManagerHandler(IWorkSpaceUserService workSpaceUserService) : AuthorizationHandler<WorkSpaceProjectManagerRequirement, long>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, WorkSpaceProjectManagerRequirement requirement, long resource)
        {
            var userId = context.User.GetUserId();
            if (userId is null)
                return;

            //check if user is project manager
            var isProjectManager = await workSpaceUserService.IsUserHasWorkSpaceRoleAsync(userId, resource, WorkSpaceRole.ProjectManager);
            if (isProjectManager)
            {
                context.Succeed(requirement);
                return;
            }

            return;
        }
    }
}
