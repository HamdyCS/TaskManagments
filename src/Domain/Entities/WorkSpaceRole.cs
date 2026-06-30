using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class WorkSpaceRole
    {
        public short Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<WorkSpaceUser> WorkSpaceUsers { get; set; }
    }
}
