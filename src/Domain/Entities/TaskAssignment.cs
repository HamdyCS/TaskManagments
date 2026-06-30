using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class TaskAssignment
    {
        public long Id { get; set; }
        public long TaskId { get; set; }
        public string AssignedById { get; set; }
        public string AssignedToId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UnassignedAt { get; set; }
        public bool IsActive { get; set; }

        public virtual ProjectTask Task { get; set; }
        public virtual User AssignedBy { get; set; }
        public virtual User AssignedTo { get; set; }
    }
}
