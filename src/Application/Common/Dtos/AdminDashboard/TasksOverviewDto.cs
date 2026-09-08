using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos.AdminDashboard
{
    public class TasksOverviewDto
    {
        public int BacklogCount { get; set; }

        public int TodoCount { get; set; }

        public int InProgressCount { get; set; }

        public int ReviewCount { get; set; }

        public int DoneCount { get; set; }

    }
}
