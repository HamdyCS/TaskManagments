using Domain.Common.Enums;

namespace Application.Features.Tasks.Commands.CreateTask
{
    public record CreateTaskDto(
        string Name,
        string? Description,
        DateTime? Deadline,
        TaskPriority Priority,
        string? AssignedUserId);
}
