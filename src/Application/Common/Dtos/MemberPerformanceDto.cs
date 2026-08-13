using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos
{
    public class MemberPerformanceDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int AssignedCount { get; set; }
        public int InProgressCount { get; set; }
        public int DoneCount { get; set; }
    }
}
