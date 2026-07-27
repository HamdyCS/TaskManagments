using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos
{
    public class MemberPerformance
    {
        public int AssignedCount { get; set; }
        public int InProgressCount { get; set; }
        public int DoneCount { get; set; }
    }
}
