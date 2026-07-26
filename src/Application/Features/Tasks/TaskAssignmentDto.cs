namespace Application.Features.Tasks
{
    public class TaskAssignmentDto
    {
        public long Id { get; set; }
        public string AssignedToId { get; set; } = string.Empty;
        public string AssignedById { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UnassignedAt { get; set; }
        public bool IsActive { get; set; }

        public TaskAssignmentDto() { }

        public TaskAssignmentDto(
            long id,
            string assignedToId,
            string assignedById,
            DateTime createdAt,
            DateTime? unassignedAt,
            bool isActive)
        {
            Id = id;
            AssignedToId = assignedToId;
            AssignedById = assignedById;
            CreatedAt = createdAt;
            UnassignedAt = unassignedAt;
            IsActive = isActive;
        }
    }
}
