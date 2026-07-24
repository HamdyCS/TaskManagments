using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Common.Enums
{
    public enum NotificationType
    {
        TaskAssigned = 1,
        TaskUnassigned,
        TaskStatusUpdated,
        TaskUpdated,
        CommentAdded,
        DueDateReminder,
        TaskDeleted,
        WorkSpaceInvite
    }
}
