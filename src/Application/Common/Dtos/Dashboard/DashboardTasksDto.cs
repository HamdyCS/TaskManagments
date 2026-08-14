using Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos.Dashboard
{
    public class DashboardTasksDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string ProjectName { get; set; }

        public TaskPriority Priority { get; set; }

        public ProjectTaskStatus Status { get; set; }


        public DateTime CreatedAt { get; set; }

        public DateTime? DeadLine { get; set; }
    }
}
