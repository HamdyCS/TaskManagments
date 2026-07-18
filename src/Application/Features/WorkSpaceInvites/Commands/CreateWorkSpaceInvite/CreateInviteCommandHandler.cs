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

namespace Application.Features.WorkSpaceInvites.Commands.CreateWorkSpaceInvite
{
    public class CreateInviteCommandHandler(IUnitOfWork unitOfWork, IMediator mediator, IConfiguration configuration,
        ILogger<CreateInviteCommandHandler> logger) :
        IRequestHandler<CreateInviteCommand, ErrorOr<WorkSpaceInviteDto>>
    {
        public async Task<ErrorOr<WorkSpaceInviteDto>> Handle(CreateInviteCommand request, CancellationToken cancellationToken)
        {
            var createWorkSpaceInviteDto = request.CreateWorkSpaceInviteDto;
            var createBy = request.UserId;
            logger.LogInformation("Starting create workspace invite to user with email " +
                "{InviteToEmail} to workspace with Id {WorkSpaceId}"
                , createWorkSpaceInviteDto.InviteToEmail, createWorkSpaceInviteDto.WorkSpaceId);


            //get invite to user
            var inviteToUser = await unitOfWork.UserRepository.GetByEmailAsync(createWorkSpaceInviteDto.InviteToEmail);
            if (inviteToUser is null)
            {
                logger.LogWarning("User with email {InviteToEmail} not found", createWorkSpaceInviteDto.InviteToEmail);
                return UserErrors.UserNotFoundByEmail(createWorkSpaceInviteDto.InviteToEmail);
            }

            //is user invited by self
            if(inviteToUser.Id == createBy)
            {
                logger.LogWarning("User with email {InviteToEmail} cannot invite his self", createWorkSpaceInviteDto.InviteToEmail);
                return WorkSpaceInviteErrors.UserCannotInviteHimself(createWorkSpaceInviteDto.InviteToEmail);
            }

            //get workspace
            var workspace = await unitOfWork.WorkSpaceRepository.GetByIdAsync(createWorkSpaceInviteDto.WorkSpaceId);
            if (workspace is null)
            {
                logger.LogWarning("Workspace with Id {WorkSpaceId} not found", createWorkSpaceInviteDto.WorkSpaceId);
                return WorkSpaceErrors.WorkSpaceNotFoundById(createWorkSpaceInviteDto.WorkSpaceId);
            }
            
            //is user already in workspace
            var isUserInWorkspace = await unitOfWork.WorkSpaceUserRepository.IsUserExistInWorkSpaceAsync(inviteToUser.Id, createWorkSpaceInviteDto.WorkSpaceId);
            if (isUserInWorkspace)
            {
                logger.LogWarning("User with email {InviteToEmail} already in workspace with Id {WorkSpaceId}",
                    createWorkSpaceInviteDto.InviteToEmail, createWorkSpaceInviteDto.WorkSpaceId);
                return WorkSpaceErrors.UserAlreadyInWorkspaceByEmail(createWorkSpaceInviteDto.InviteToEmail, createWorkSpaceInviteDto.WorkSpaceId);
            }

            //is user has already pending invite to workspace
            var isInvited = await unitOfWork.WorkSpaceInviteRepository.IsUserHasValidWorkSpaceInviteByStatusAsync(
                inviteToUser.Id,
                createWorkSpaceInviteDto.WorkSpaceId,
                WorkSpaceInviteStatus.Pending
                );

            if (isInvited)
            {
                logger.LogWarning("User with email {InviteToEmail} already invited to workspace with Id {WorkSpaceId}",
                    createWorkSpaceInviteDto.InviteToEmail, createWorkSpaceInviteDto.WorkSpaceId);
                return WorkSpaceInviteErrors.UserAlreadyHasPendingInvites(createWorkSpaceInviteDto.InviteToEmail, createWorkSpaceInviteDto.WorkSpaceId);
            }

            //create workspace invite
            var newWorkSpaceInvite = createWorkSpaceInviteDto.Adapt<WorkSpaceInvite>();

            newWorkSpaceInvite.InvitedById = createBy;
            newWorkSpaceInvite.CreatedAt = DateTime.UtcNow;
            newWorkSpaceInvite.WorkSpaceInviteStatus = WorkSpaceInviteStatus.Pending;
            newWorkSpaceInvite.InvitedToId = inviteToUser.Id;

            var inviteLifeTimeDays = configuration.GetValue<long>("WorkSpaceInvite:LifeTimeDays");
            newWorkSpaceInvite.ExpiresAt = DateTime.UtcNow.AddDays(inviteLifeTimeDays);

            //add workspace invite

            logger.LogInformation("Adding workspace invite to user with email " +
                "{InviteToEmail} to workspace with Id {WorkSpaceId}", createWorkSpaceInviteDto.InviteToEmail, createWorkSpaceInviteDto.WorkSpaceId);

            unitOfWork.WorkSpaceInviteRepository.Add(newWorkSpaceInvite);
            var isAdded = await unitOfWork.SaveChangesAsync(cancellationToken) > 0;

            if (!isAdded)
            {
                logger.LogWarning("Failed to add workspace invite to user with email " +
                "{InviteToEmail} to workspace with Id {WorkSpaceId}", createWorkSpaceInviteDto.InviteToEmail, createWorkSpaceInviteDto.WorkSpaceId);

                return WorkSpaceInviteErrors.CreateWorkSpaceInviteFailed(createWorkSpaceInviteDto.InviteToEmail, createWorkSpaceInviteDto.WorkSpaceId);
            }

            //create notification
            var createNotificationCommand = new CreateNotificationDto(
                Message: $"You have been invited to workspace with Id {createWorkSpaceInviteDto.WorkSpaceId}",
                NotificationType: NotificationType.WorkSpaceInvite,
                Title: "WorkSpace Invite",
                NotifyToId: inviteToUser.Id,
                WorkSpaceInviteId: newWorkSpaceInvite.Id,
                TaskId: null
                );

            await mediator.Send(new CreateNotificationCommand(createNotificationCommand), cancellationToken);

            logger.LogInformation("Added workspace invite to user with email " +
                "{InviteToEmail} to workspace with Id {WorkSpaceId} successfully", createWorkSpaceInviteDto.InviteToEmail, createWorkSpaceInviteDto.WorkSpaceId);
            return newWorkSpaceInvite.Adapt<WorkSpaceInviteDto>();
        }
    }

}