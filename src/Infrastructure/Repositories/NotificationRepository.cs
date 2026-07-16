using Application.Common.Interfaces.Repositories;
using Domain.Common.Pagination;
using Domain.Entities;
using Infrastructure.Persistence;
 using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class NotificationRepository(AppDbContext context) : GenericRepository<Notification>(context), INotificationRepository
    {
        public async Task<Notification?> GetNotificationByIdAndUserIdAsync(long Id, string userId)
        {
            var notification = await GetByFilterAsync(n => n.Id == Id && n.NotifyToId == userId);
            return notification;
        }

        public async Task<PaginationResult<Notification>> GetAllUnReadUserNotificationsAsync(string userId,int pageNumber, int pageSize)
        {
            var query = context.Notifications.Where(n => n.NotifyToId == userId && !n.IsRead);

            var totalCount = await query.CountAsync();

            var notifications = query.
                OrderByDescending(n => n.CreatedAt).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PaginationResult<Notification>(notifications, totalCount, pageNumber, pageSize);
        }

        public async Task<PaginationResult<Notification>> GetAllUserNotificationsAsync(string userId, int pageNumber, int pageSize)
        {
            var query = context.Notifications.Where(n => n.NotifyToId == userId);

            var totalCount = await query.CountAsync();

            var notifications = query.
                OrderByDescending(n => n.CreatedAt).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PaginationResult<Notification>(notifications, totalCount, pageNumber, pageSize);
        }
    }
}
