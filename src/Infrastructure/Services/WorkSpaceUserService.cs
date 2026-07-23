using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Domain.Common.Enums;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Services
{
    public class WorkSpaceUserService(IUnitOfWork unitOfWork, ILogger<WorkSpaceUserService> logger) : IWorkSpaceUserService
    {
        public async Task<bool> AddUserToWorkSpaceAsync(string userId, long workSpaceId, WorkSpaceRole workSpaceRole)
        {
            logger.LogInformation("Starting add user with id {userId} to workSpace with id {WorkSpaceId} with role {workSpaceRole}", userId, workSpaceId, workSpaceRole);

            //check if user exist in workSpace
            var isUserExistInWorkSpace = await unitOfWork.WorkSpaceUserRepository.IsUserExistInWorkSpaceAsync(userId, workSpaceId);
            if (isUserExistInWorkSpace)
            {
                logger.LogWarning("User with id {userId} is already in workSpace with id {WorkSpaceId}", userId, workSpaceId);
                return false;
            }

            //add user to workSpace
            var workSpaceUser = new WorkSpaceUser
            {
                UserId = userId,
                WorkSpaceId = workSpaceId,
                WorkSpaceRole = workSpaceRole,
                CreatedAt = DateTime.UtcNow,
            };

            logger.LogInformation("Adding user with id {userId} to workSpace with id {WorkSpaceId} with role {workSpaceRole}", userId, workSpaceId, workSpaceRole);
            unitOfWork.WorkSpaceUserRepository.Add(workSpaceUser);
            var isAdded = await unitOfWork.SaveChangesAsync() > 0;

            if(!isAdded)
            {
                logger.LogWarning("Failed to add user with id {userId} to workSpace with id {WorkSpaceId} with role {workSpaceRole}", userId, workSpaceId, workSpaceRole);
                return false;
            }

         
            logger.LogInformation("Added user with id {userId} to workSpace with id {WorkSpaceId} with role {workSpaceRole} successfully", userId, workSpaceId, workSpaceRole);
            return true;
        }

        public async Task<bool> IsInWorkSpaceAsync(string userId, long workSpaceId)
        {
            logger.LogInformation("Starting check if user with id {userId} is in workSpace with id {WorkSpaceId}", userId, workSpaceId);

            logger.LogInformation("Getting check if user with id {userId} is in workSpace with id {WorkSpaceId}", userId, workSpaceId);
            var isUserExistInWorkSpace = await unitOfWork.WorkSpaceUserRepository.IsUserExistInWorkSpaceAsync(userId, workSpaceId);

            logger.LogInformation("Checked if user with id {userId} is in workSpace with id {WorkSpaceId} successfully", userId, workSpaceId);
            return isUserExistInWorkSpace;
        }

        public async Task<bool> IsUserHasWorkSpaceRoleAsync(string userId, long workSpaceId, WorkSpaceRole workSpaceRole)
        {

            logger.LogInformation("Starting check if user with id {userId} is in workSpace with id {WorkSpaceId} has role {workSpaceRole}", userId, workSpaceId, workSpaceRole);

            logger.LogInformation("Getting check if user with id {userId} is in workSpace with id {WorkSpaceId} has role {workSpaceRole}", userId, workSpaceId, workSpaceRole);
            var isUserHasWorkSpaceRole = await unitOfWork.WorkSpaceUserRepository.IsUserHasWorkSpaceRoleAsync(userId, workSpaceId, workSpaceRole);

            logger.LogInformation("Checked if user with id {userId} is in workSpace with id {WorkSpaceId} has role {workSpaceRole} successfully", userId, workSpaceId, workSpaceRole);
            return isUserHasWorkSpaceRole;
        }
    }
}
