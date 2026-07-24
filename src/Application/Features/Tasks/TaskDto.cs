using Domain.Common.Enums;

namespace Application.Features.Tasks
{
    public record TaskDto(
        long Id,
        string Name,
        string? Description,
        DateTime? Deadline,
        ProjectTaskStatus TaskStatus,
        TaskPriority TaskPriority,
        DateTime CreatedAt,
        DateTime? LastUpdatedAt,
        string? LastUpdatedById,
        long ProjectId,
        string CreatedById,
        List<TaskAssignmentDto> Assignments);
}
