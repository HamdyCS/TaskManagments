using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IRefreshTokenRepository RefreshTokenRepository { get; }
        IWorkSpaceRepository WorkSpaceRepository { get; }
        INotificationRepository NotificationRepository { get; }
        IWorkSpaceUserRepository WorkSpaceUserRepository { get; }
        IWorkSpaceInviteRepository WorkSpaceInviteRepository { get; }
        IProjectRepository ProjectRepository { get; }
        ITaskRepository TaskRepository { get; }
        ITaskAssignmentRepository TaskAssignmentRepository { get; }
        ITaskAttachmentRepository TaskAttachmentRepository { get; }
        ITaskCommentRepository TaskCommentRepository { get; }
        IReportRepository ReportRepository { get; }

        IDashboardRepository DashboardRepository { get; }


        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
        Task DisposeAsync();
    }
}
