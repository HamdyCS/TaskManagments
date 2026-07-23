using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Domain.Common.Enums;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaces.commands.UpdateWorkSpace
{
    public class UpdateWorkSpaceCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateWorkSpaceCommandHandler> logger
        ) : IRequestHandler<UpdateWorkSpaceCommand, ErrorOr<bool>>

    {
        public async Task<ErrorOr<bool>> Handle(UpdateWorkSpaceCommand request, CancellationToken cancellationToken)
        {
            var updateWorkSpaceDto = request.UpdateWorkSpaceDto;
            var updateBy = request.UpdateBy;
            var workSpaceId = request.WorkSpaceId;
            logger.LogInformation("Starting update workspace with id {WorkSpaceId} by user with id {UserId}", workSpaceId,updateBy);

            var workSpace = await unitOfWork.WorkSpaceRepository.GetByIdAsync(workSpaceId);
            if(workSpace is null)
            {
                logger.LogWarning("Workspace with id {WorkSpaceId} not found", workSpaceId);
                return WorkSpaceErrors.WorkSpaceNotFoundById(workSpaceId);
            }

            //update workspace
            logger.LogInformation("updating workspace with id {WorkSpaceId} by user with id {UserId}", workSpaceId, updateBy);

            updateWorkSpaceDto.Adapt(workSpace);
            workSpace.LastUpdatedById = updateBy;
            workSpace.LastUpdatedAt = DateTime.UtcNow;

            
            unitOfWork.WorkSpaceRepository.Update(workSpace);

            var isAddedWorkUpdated = await unitOfWork.SaveChangesAsync(cancellationToken) > 0;

            if(!isAddedWorkUpdated)
            {
                logger.LogWarning("Failed to update workspace with id {WorkSpaceId} by user with id {UserId}", workSpaceId, updateBy);
                return WorkSpaceErrors.UpdateWorkSpaceFailed(workSpaceId, updateBy);
            }

            
            logger.LogInformation("Updated workspace with id {WorkSpaceId} by user with id {UserId} ", workSpaceId, updateBy);
            return true;
            
        }
    }
}
