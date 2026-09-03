using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos
{
    public class WorkSpaceReportDto
    {
        public string WorkSpaceName { get; set; }

        public IEnumerable<string> OwnerNames { get; set; }

        public int TotalProjects { get; set; }

        public int TotalMembers { get; set; }
        public int TotalTasks { get; set; }
        public int TotalBacklogTasks { get; set; }
        public int TotalTodoTasks { get; set; }
        public int TotalInProgressTasks { get; set; }

        public int TotalReviewTasks { get; set; }
        public int TotalDoneTasks { get; set; }

        public double CompletionPercentage {  get; set; }
        public IEnumerable<MemberPerformanceDto> MemberPerformances { get; set; }

    }
}
