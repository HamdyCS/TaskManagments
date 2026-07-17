using Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaceInvites
{
    public class WorkSpaceInviteDto
    {
        public long Id { get; set; }

        public long WorkSpaceId { get; set; }

        public string InitedToId { get; set; }

        public string InvitedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ExpiresAt { get; set; }

        public WorkSpaceInviteStatus WorkSpaceInviteStatus { get; set; }
    }
}
