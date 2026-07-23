using Application.Common.Interfaces.Services;

namespace Api.Polices.WorkSpace.WorkSpaceUser
{
    public class WorkSpaceUserHandler(IWorkSpaceUserService workSpaceUserService) : AuthorizationHandler<WorkSpaceUserRequirement, long>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, WorkSpaceUserRequirement requirement, long resource)
        {
            var userId = context.User.GetUserId();
            if (userId is null)
                return;

            var isUserInWorkSpace = await workSpaceUserService.IsInWorkSpaceAsync(userId, resource);
            if (isUserInWorkSpace)
                context.Succeed(requirement);


            return;
        }
    }
}
