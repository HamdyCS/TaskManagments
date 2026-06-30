using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class TaskComment
    {
        public long Id { get; set; }
        public string Comment { get; set; }
        public long TaskId { get; set; }
        public string CommentById { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual ProjectTask Task { get; set; }
        public virtual User CommentBy { get; set; }
    }
}
