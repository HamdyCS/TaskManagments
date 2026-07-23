using Domain.Common.Enums;

namespace Application.Features.Projects
{
    public record ProjectDto(
        long Id,
        string Name,
        string? Description,
        ProjectStatus Status,
        long WorkSpaceId,
        string CreatedById,
        DateTime CreatedAt,
        string? LastUpdatedById,
        DateTime? LastUpdatedAt);
}
