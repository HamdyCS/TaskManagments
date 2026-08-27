using Domain.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class TaskAttachment : IBaseEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string StorageKey { get; set; }
        public decimal Size { get; set; }
        public string ContentType { get; set; }
        public long TaskId { get; set; }
        public string CreatedById { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual ProjectTask Task { get; set; }

        public virtual User CreatedBy { get; set; }
    }
}
