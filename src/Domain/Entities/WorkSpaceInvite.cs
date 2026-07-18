using Domain.Common.Enums;
using Domain.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class WorkSpaceInvite : IBaseEntity,ISoftDelete
    {
        public long Id { get; set; }

        public long WorkSpaceId { get; set; }

        public string InvitedToId { get; set; }

        public string InvitedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ExpiresAt { get; set; }


        public WorkSpaceRole WorkSpaceRole { get; set; }

        [NotMapped]
        public bool IsExpired => DateTime.UtcNow > ExpiresAt;

        public WorkSpaceInviteStatus WorkSpaceInviteStatus { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public virtual WorkSpace WorkSpace { get; set; }

        public virtual User InvitedBy { get; set; }

        public virtual User InvitedTo { get; set; }


        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
