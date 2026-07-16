using Domain.Common.Enums;
using Domain.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Notification : IBaseEntity
    {
        public long Id { get; set; }
        public NotificationType NotificationType { get; set; }
        public string NotifyToId { get; set; }
        public long? TaskId { get; set; }
        public long? WorkSpaceInviteId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReadAt { get; set; } 
        public virtual User NotifyTo { get; set; }
        public virtual ProjectTask Task { get; set; }

        public virtual WorkSpaceInvite WorkSpaceInvite { get; set; }
    }
}
