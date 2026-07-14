using Domain.Common.Pagination;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces.Repositories
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        public Task<PaginationResult<Notification>> GetAllUserNotificationsAsync(string userId, int pageNumber, int pageSize);

        public Task<PaginationResult<Notification>> GetAllUnReadUserNotificationsAsync(string userId,int pageNumber,int pageSize);

        public Task<Notification?> GetNotificationByIdAndUserIdAsync(long Id,string userId);

    }
}
