using Application.Common.Interfaces.Services;

namespace Api.Polices.WorkSpace.WorkSpaceOwner
{
    public class WorkSpaceOwnerHandler(IWorkSpaceUserService workSpaceUserService) : AuthorizationHandler<WorkSpaceOwnerRequirement, long>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, WorkSpaceOwnerRequirement requirement, long resource)
        {
            var userId = context.User.GetUserId();
            if (userId is null)
                return;

            var isOwner = await workSpaceUserService.IsUserHasWorkSpaceRoleAsync(userId, resource, WorkSpaceRole.Owner);
            if (isOwner)
                context.Succeed(requirement);


            return;
        }
    }
}
