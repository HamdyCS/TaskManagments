using Application.Common.Interfaces.Repositories;
using Domain.Common.Pagination;
using Domain.Entities;
using Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class NotificationRepository(AppDbContext context) : GenericRepository<Notification>(context), INotificationRepository
    {
        public async Task<Notification?> GeNotificationByIdAndUserIdAsync(long Id, string userId)
        {
            var notification = await GetByFilterAsync(n => n.Id == Id && n.NotifyToId == userId);
            return notification;
        }

        public Task<PaginationResult<Notification>> GetAllUnReadUserNotificationsAsync(string userId)
        {
            var notifications = 
                GetAllByFilterAsync(n => n.NotifyToId == userId && !n.IsRead);

            return notifications;
        }

        public Task<PaginationResult<Notification>> GetAllUserNotificationsAsync(string userId)
        {
            var notifications = 
                GetAllByFilterAsync(n => n.NotifyToId == userId);

            return notifications;
        }
    }
}
