namespace Application.Features.TaskComments
{
    public class TaskCommentDto
    {
        public long Id { get; set; }
        public string Comment { get; set; }
        public long TaskId { get; set; }
        public string CommentById { get; set; }
        public string CommentByName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }

       
    }
}
