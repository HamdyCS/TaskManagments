using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Domain.Common.Pagination;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Services
{
    public class RecentActivityService(IUnitOfWork unitOfWork,
        ILogger<RecentActivityService> logger) : IRecentActivityService
    {
        public async Task<bool> AddAsync(RecentActivity recentActivity)
        {
            logger.LogInformation("Starting Add New Recent Activity with ActivityType: {ActivityType} at {Timestamp}", recentActivity.ActivityType, DateTime.UtcNow);

            logger.LogInformation("Adding New Recent Activity with ActivityType: {ActivityType} at {Timestamp}", recentActivity.ActivityType, DateTime.UtcNow);
            unitOfWork.RecentActivityRepository.Add(recentActivity);

            var isAdded = await unitOfWork.SaveChangesAsync() > 0;
            if (isAdded)
            {
                logger.LogInformation("Successfully Added New Recent Activity ActivityType: {ActivityType} at {Timestamp}", recentActivity.ActivityType, DateTime.UtcNow);

            }
            else
            {
                logger.LogError("Failed to Add New Recent Activity with ActivityType: {ActivityType} at {Timestamp}", recentActivity.ActivityType, DateTime.UtcNow);
            }

           return isAdded;
        }

        public async Task<PaginationResult<RecentActivity>> GetAllAsync(int pageNumber, int pageSize)
        {
            logger.LogInformation("Starting Get All Recent Activities at {Timestamp}", DateTime.UtcNow);

            logger.LogInformation("Getting All Recent Activities at {Timestamp}", DateTime.UtcNow);
            var recentActivities = await unitOfWork.RecentActivityRepository.GetAllAsync(pageNumber, pageSize);

            logger.LogInformation("Successfully Got All Recent Activities at {Timestamp}", DateTime.UtcNow);
            return recentActivities;
        }
    }
}
