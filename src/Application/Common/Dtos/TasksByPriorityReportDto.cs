using Domain.Common.Enums;

namespace Application.Common.Dtos
{
    public class TasksByPriorityReportDto
    {
        public TaskPriority TaskPriority { get; set; }
        public int Count { get; set; }
    }
}
