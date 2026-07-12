using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Domain.Common.Enums;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaces.commands.CreateWorkSpace
{
    public class CreateWorkSpaceCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateWorkSpaceCommandHandler> logger
        ) : IRequestHandler<CreateWorkSpaceCommand, ErrorOr<WorkSpaceDto>>

    {
        public async Task<ErrorOr<WorkSpaceDto>> Handle(CreateWorkSpaceCommand request, CancellationToken cancellationToken)
        {
            var createWorkSpaceDto = request.CreateWorkSpaceDto;
            var createBy = request.CreateBy;
            logger.LogInformation("Starting create workspace for user with id {UserId}", createBy);

            var workSpace = createWorkSpaceDto.Adapt<WorkSpace>();
            workSpace.CreatedById = createBy;
            workSpace.CreatedAt = DateTime.UtcNow;

            await unitOfWork.BeginTransactionAsync(cancellationToken);

            logger.LogInformation("Adding workspace for user with id {UserId}", createBy);

            //Add workspace
            unitOfWork.WorkSpaceRepository.Add(workSpace);

            var isAddedWorkAdded = await unitOfWork.SaveChangesAsync(cancellationToken) > 0;

            if(!isAddedWorkAdded)
            {
                logger.LogWarning("Failed to add workspace for user with id {UserId}", createBy);
                return WorkSpaceErrors.CreateWorkSpaceFailed(createBy);
            }

            //add user to workspace
            logger.LogInformation("Adding user to workspace for user with id {UserId}", createBy);
            var workspaceUser = new WorkSpaceUser
            {
                UserId = createBy,
                WorkSpaceId = workSpace.Id,
                WorkSpaceRoleId = (int)WorkSpaceRole.Owner
            };

            unitOfWork.WorkSpaceUserRepository.Add(workspaceUser);
            var isAddedUserToWorkspace = await unitOfWork.SaveChangesAsync(cancellationToken) > 0;
            if(!isAddedUserToWorkspace)
            {
                logger.LogWarning("Failed to add user to workspace for user with id {UserId}", createBy);
                return WorkSpaceErrors.CreateWorkSpaceFailed(createBy);
            }

            await unitOfWork.CommitTransactionAsync(cancellationToken);

            logger.LogInformation("Added workspace for user with id {UserId} successfully", createBy);
            return workSpace.Adapt<WorkSpaceDto>();
            
        }
    }
}
