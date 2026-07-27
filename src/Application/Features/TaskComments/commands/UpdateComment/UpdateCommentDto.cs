namespace Application.Features.TaskComments.Commands.UpdateComment
{
    public sealed record UpdateCommentDto
    {
        public string Comment { get; init; } = string.Empty;
    }
}
