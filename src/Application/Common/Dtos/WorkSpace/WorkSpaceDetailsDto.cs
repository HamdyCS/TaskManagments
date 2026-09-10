using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos.WorkSpace
{
    public class WorkSpaceDetailsDto
    {
        
        public WorkSpaceOverviewDto WorkSpaceOverview { get; set; }

        public double CompletionPercentage { get; set; }

        public IEnumerable<MemberDto> Members { get; set; }
        
        public IEnumerable<string> ProjectNames { get; set; }
    }
}
