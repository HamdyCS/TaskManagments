using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class ProjectTaskStatus
    {
        public short Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<ProjectTask> Tasks { get; set; }
    }
}
