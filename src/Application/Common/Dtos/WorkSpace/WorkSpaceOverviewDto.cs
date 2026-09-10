using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos.WorkSpace
{
    public class WorkSpaceOverviewDto
    {
        public long Id { get; set; }

        public IEnumerable<string> OwnersNames { get; set; }

        public int MembersCount { get; set; }

        public int ProjectsCount { get; set; }

        public int TasksCount { get; set; }

        public DateTime CreatedAt { get; set; }
        public string Name { get; set; }
    }
}
