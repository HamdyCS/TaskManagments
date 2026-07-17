using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Application.Features.Notifications.Command.CreateNotification;
using Domain.Common.Enums;
using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaceInvites.Commands.UpdateWorkSpaceInviteStatus
{
    public class UpdateInviteStatusCommandHandler(IUnitOfWork unitOfWork, IMediator mediator,
        IWorkSpaceUserService workSpaceUserService,
        ILogger<UpdateInviteStatusCommandHandler> logger) :
        IRequestHandler<UpdateInviteStatusCommand, ErrorOr<bool>>
    {
        public async Task<ErrorOr<bool>> Handle(UpdateInviteStatusCommand request, CancellationToken cancellationToken)
        {
            var workSpaceInviteId = request.WorkSpaceInviteId;
            var inviteToId = request.InviteToId;
            var workSpaceInviteStatus = request.WorkSpaceInviteStatus;

            logger.LogInformation("Starting updating workspace invite with Id {WorkSpaceInviteId} to status {WorkSpaceInviteStatus} for invite to user with Id {InviteToId}", workSpaceInviteId, workSpaceInviteStatus, inviteToId);

            //get workspace invite
            var workSpaceInvite = await unitOfWork.WorkSpaceInviteRepository.GetWorkSpaceInviteByIdAndInviteByIdAsync(workSpaceInviteId, inviteToId);
            if (workSpaceInvite is null)
            {
                logger.LogWarning("WorkSpaceInvite with Id {WorkSpaceInviteId} not found for invite to user with Id {InviteToId}", workSpaceInviteId, inviteToId);
                return WorkSpaceInviteErrors.WorkSpaceInviteNotFoundByIdAndInvitedToId(workSpaceInviteId, inviteToId);
            }

            //is workspace invite not pending
            if (workSpaceInvite.WorkSpaceInviteStatus != WorkSpaceInviteStatus.Pending)
            {
                logger.LogWarning("WorkSpaceInvite with Id {WorkSpaceInviteId} is not pending for invite to user with Id {InviteToId}", workSpaceInviteId, inviteToId);
                return WorkSpaceInviteErrors.WorkSpaceInviteIsNotPendingByInviteTo(workSpaceInviteId, inviteToId);
            }

            //is workspace invite expired
            if (workSpaceInvite.IsExpired)
            {
                logger.LogWarning("WorkSpaceInvite with Id {WorkSpaceInviteId} is expired for invite to user with Id {InviteToId}", workSpaceInviteId, inviteToId);
                return WorkSpaceInviteErrors.WorkSpaceInviteExpired(workSpaceInviteId, inviteToId);
            }

            //begin transaction
            await unitOfWork.BeginTransactionAsync(cancellationToken);

            //update workspace invite status
            workSpaceInvite.WorkSpaceInviteStatus = workSpaceInviteStatus;
            unitOfWork.WorkSpaceInviteRepository.Update(workSpaceInvite);

            var isWorkSpaceInviteUpdated = await unitOfWork.SaveChangesAsync(cancellationToken) >0;
            if (!isWorkSpaceInviteUpdated)
            {
                logger.LogWarning("WorkSpaceInvite with Id {WorkSpaceInviteId} not updated for invite to user with Id {InviteToId}", workSpaceInviteId, inviteToId);
                return WorkSpaceInviteErrors.UpdateWorkSpaceStatusInviteFailed(workSpaceInviteId, inviteToId, workSpaceInviteStatus);
            }

            //is workspace invite accepted
            if (workSpaceInviteStatus == WorkSpaceInviteStatus.Accepted)
            {
                var workSpaceName = await unitOfWork.WorkSpaceRepository.GetWorkSpaceNameAsync(workSpaceInvite.WorkSpaceId);
                //add user to workspace
               var isAddedToWorkSpace = await workSpaceUserService.AddUserToWorkSpaceAsync(inviteToId,workSpaceInvite.WorkSpaceId, 
                  workSpaceInvite.WorkSpaceRole);

                if(!isAddedToWorkSpace)
                {
                    return WorkSpaceErrors.AddUserToWorkSpaceFailed( inviteToId, workSpaceInvite.WorkSpaceId);
                }

                //create notification
                var notification = new CreateNotificationDto(inviteToId,null,workSpaceInviteId,
                    $"You have been added to workspace {workSpaceName}" ,"WorkSpace Invite", NotificationType.WorkSpaceInvite);

                await mediator.Send(new CreateNotificationCommand(notification), cancellationToken);   
            }

            //commit transaction
            await unitOfWork.CommitTransactionAsync(cancellationToken);

            logger.LogInformation("WorkSpaceInvite with Id {WorkSpaceInviteId} updated to status {WorkSpaceInviteStatus} for invite to user with Id {InviteToId} successfully", workSpaceInviteId, workSpaceInviteStatus, inviteToId);
            return true;

        }
    }

}