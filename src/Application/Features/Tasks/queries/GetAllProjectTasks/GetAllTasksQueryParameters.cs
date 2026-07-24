using Domain.Common.Enums;

namespace Application.Features.Tasks.Queries.GetAllProjectTasks
{
    public record GetAllTasksQueryParameters(
        ProjectTaskStatus? Status,
        TaskPriority? Priority,
        string? SearchTerm,
        string? SortBy,
        string? SortOrder);
}
