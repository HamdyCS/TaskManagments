using Application.Features.TaskAttachments;
using Domain.Common.Enums;

namespace Application.Features.Tasks
{
    public class TaskDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? Deadline { get; set; }
        public ProjectTaskStatus TaskStatus { get; set; }
        public TaskPriority TaskPriority { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public string? LastUpdatedById { get; set; }
        public long ProjectId { get; set; }
        public string CreatedById { get; set; } = string.Empty;
        public List<TaskAssignmentDto> Assignments { get; set; } = new();
        public List<TaskAttachmentDto> Attachments { get; set; } = new();

        public TaskDto() { }

        public TaskDto(
            long id,
            string name,
            string? description,
            DateTime? deadline,
            ProjectTaskStatus taskStatus,
            TaskPriority taskPriority,
            DateTime createdAt,
            DateTime? lastUpdatedAt,
            string? lastUpdatedById,
            long projectId,
            string createdById,
            List<TaskAssignmentDto> assignments,
            List<TaskAttachmentDto> attachments)
        {
            Id = id;
            Name = name;
            Description = description;
            Deadline = deadline;
            TaskStatus = taskStatus;
            TaskPriority = taskPriority;
            CreatedAt = createdAt;
            LastUpdatedAt = lastUpdatedAt;
            LastUpdatedById = lastUpdatedById;
            ProjectId = projectId;
            CreatedById = createdById;
            Assignments = assignments;
            Attachments = attachments;
        }
    }

}
