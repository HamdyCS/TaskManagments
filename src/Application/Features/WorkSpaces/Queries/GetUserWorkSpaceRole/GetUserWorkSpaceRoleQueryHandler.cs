using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Domain.Common.Enums;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.WorkSpaces.Queries.GetUserWorkSpaceRole
{
    public class GetUserWorkSpaceRoleQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetUserWorkSpaceRoleQueryHandler> logger) : IRequestHandler<GetUserWorkSpaceRoleQuery, ErrorOr<WorkSpaceRole>>
    {
        public async Task<ErrorOr<WorkSpaceRole>> Handle(GetUserWorkSpaceRoleQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting get user role in workspace {WorkSpaceId} for user {UserId}", request.WorkSpaceId, request.UserId);

            var workSpaceUser = await unitOfWork.WorkSpaceUserRepository.GetWorkSpaceUserAsync(request.UserId, request.WorkSpaceId);

            if (workSpaceUser is null)
            {
                logger.LogInformation("User {UserId} is not a member of workspace {WorkSpaceId}", request.UserId, request.WorkSpaceId);
                return WorkSpaceErrors.UserNotInWorkspace(request.UserId, request.WorkSpaceId);
            }

            logger.LogInformation("User {UserId} has role {Role} in workspace {WorkSpaceId}", request.UserId, workSpaceUser.WorkSpaceRole, request.WorkSpaceId);
            return workSpaceUser.WorkSpaceRole;
        }
    }
}
