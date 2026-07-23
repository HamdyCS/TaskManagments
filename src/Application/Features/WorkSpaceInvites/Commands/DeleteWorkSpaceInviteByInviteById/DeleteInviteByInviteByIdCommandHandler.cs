using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Application.Features.Notifications.Command.CreateNotification;
using Domain.Common.Enums;
using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaceInvites.Commands.DeleteWorkSpaceInviteByInviteById
{
    public class DeleteInviteByInviteByIdCommandHandler(IUnitOfWork unitOfWork, IMediator mediator, IConfiguration configuration,
        ILogger<DeleteInviteByInviteByIdCommandHandler> logger) :
        IRequestHandler<DeleteInviteByInviteByIdCommand, ErrorOr<bool>>
    {
        public async Task<ErrorOr<bool>> Handle(DeleteInviteByInviteByIdCommand request, CancellationToken cancellationToken)
        {
            var workSpaceInviteId = request.WorkSpaceInviteId;
            var inviteById = request.InviteById;

            logger.LogInformation("Starting delete workspace invite with Id {WorkSpaceInviteId} for Invited by user with id {UserId}", workSpaceInviteId, inviteById);

            //get workspace invite
            var workSpaceInvite = await unitOfWork.WorkSpaceInviteRepository.GetWorkSpaceInviteByIdAndInviteByIdAsync(workSpaceInviteId, inviteById);
            if (workSpaceInvite is null)
            {
                logger.LogWarning("Workspace invite with Id {WorkSpaceInviteId} not found for Invited by user with id {InviteById}", workSpaceInviteId, inviteById);
                return WorkSpaceInviteErrors.WorkSpaceInviteNotFoundByIdAndInviteById(workSpaceInviteId, inviteById);
            }

            //is workspace invite pending
            if (workSpaceInvite.WorkSpaceInviteStatus != WorkSpaceInviteStatus.Pending)
            {
                logger.LogWarning("Workspace invite with Id {WorkSpaceInviteId} is not pending for Invited by user with id {InviteById}", workSpaceInviteId, inviteById);
                return WorkSpaceInviteErrors.WorkSpaceInviteIsNotPending(workSpaceInviteId, inviteById);
            }

            //delete workspace invite

            logger.LogInformation("Deleting workspace invite with Id {WorkSpaceInviteId} for Invited by user with id {InviteById}", workSpaceInviteId, inviteById);

            unitOfWork.WorkSpaceInviteRepository.Delete(workSpaceInvite);
            var isDeleted = await unitOfWork.SaveChangesAsync(cancellationToken) > 0;

            if(!isDeleted)
            {
                logger.LogWarning("Workspace invite with Id {WorkSpaceInviteId} not deleted for Invited by user with id {InviteById}", workSpaceInviteId, inviteById);
                return WorkSpaceInviteErrors.DeleteWorkSpaceInviteFailed(workSpaceInviteId, inviteById);
            }

            logger.LogInformation("Workspace invite with Id {WorkSpaceInviteId} deleted for Invited by user with id {InviteById} successfully", workSpaceInviteId, inviteById);

            return true;
        }
    }

}