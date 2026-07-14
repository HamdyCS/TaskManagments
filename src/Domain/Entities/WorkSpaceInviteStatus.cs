using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class WorkSpaceInviteStatus
    {
        public short Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<WorkSpaceInvite> WorkSpaceInvites { get; set; }
    }
}
