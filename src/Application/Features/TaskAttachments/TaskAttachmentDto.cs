namespace Application.Features.TaskAttachments
{
    public class TaskAttachmentDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public TaskAttachmentDto() { }

        public TaskAttachmentDto(
            long id,
            string name,
            string url,
            DateTime createdAt)
        {
            Id = id;
            Name = name;
            Url = url;
            CreatedAt = createdAt;
        }
    }

}
