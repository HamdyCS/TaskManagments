using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos.WorkSpacesOverview
{
    public class WorkSpacesOverviewReportDto
    {
        public int RegularUsersCount { get; set; }

        public int WorkspacesCount { get; set; }

        public int ProjectsCount { get; set; }

        public int TasksCount { get; set; }

        public IEnumerable<TasksByPriorityReportDto> TasksByPriorityReportDtos {  get; set; }

        public IEnumerable<TasksByStatusReportDto> TasksByStatusReportDtos { get; set; }
    }
}
