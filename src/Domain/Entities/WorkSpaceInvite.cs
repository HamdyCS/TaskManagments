using Domain.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class WorkSpaceInvite : IBaseEntity
    {
        public long Id { get; set; }

        public long WorkSpaceId { get; set; }

        public string InitedToId { get; set; }

        public string InvitedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ExpiresAt { get; set; }

        public short InviteStatusId { get; set; }

        public virtual WorkSpace WorkSpace { get; set; }

        public virtual User InvitedBy { get; set; }

        public virtual User InitedTo { get; set; }

        public virtual WorkSpaceInviteStatus InviteStatus { get; set; }

        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
