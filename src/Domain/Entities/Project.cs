using Domain.Common.Enums;
using Domain.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Project : ISoftDelete , IBaseEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ProjectStatus Status { get; set; }
        public long WorkSpaceId { get; set; }
        public string CreatedById { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? LastUpdatedById { get; set; }
        public DateTime? LastUpdatedAt { get; set; }

        public virtual WorkSpace WorkSpace { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual User? LastUpdatedBy { get; set; }
        public virtual ICollection<ProjectTask> Tasks { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}