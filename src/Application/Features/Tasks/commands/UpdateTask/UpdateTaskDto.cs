using Domain.Common.Enums;

namespace Application.Features.Tasks.Commands.UpdateTask
{
    public record UpdateTaskDto(
        string? Name,
        string? Description,
        DateTime? Deadline,
        TaskPriority? Priority);
}
