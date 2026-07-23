using Domain.Common.Enums;
using Domain.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class ProjectTask : ISoftDelete, IBaseEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long ProjectId { get; set; }
        public string CreatedById { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public ProjectTaskStatus TaskStatus { get; set; }
        public TaskPriority TaskPriority { get; set; }

        public virtual Project Project { get; set; }
        public virtual User CreatedBy { get; set; }

        public virtual ICollection<TaskAssignment> TaskAssignments { get; set; }
        public virtual ICollection<TaskComment> TaskComments { get; set; }
        public virtual ICollection<TaskAttachment> TaskAttachments { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
