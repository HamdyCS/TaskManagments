using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Project
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long WorkSpaceId { get; set; }
        public string CreatedById { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public virtual WorkSpace WorkSpace { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual ICollection<ProjectTask> Tasks { get; set; }
    }
}
