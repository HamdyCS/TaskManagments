namespace Application.Features.Tasks
{
    public record TaskAssignmentDto(
        long Id,
        string AssignedToId,
        string AssignedById,
        DateTime CreatedAt,
        DateTime? UnassignedAt,
        bool IsActive);
}
