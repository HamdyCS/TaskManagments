using Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos
{
    public class TasksByStatusReportDto
    {
        public ProjectTaskStatus TaskStatus { get; set; }
        public int Count { get; set; }
    }
}
