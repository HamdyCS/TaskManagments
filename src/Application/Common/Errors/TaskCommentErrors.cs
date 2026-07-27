using ErrorOr;

namespace Application.Common.Errors
{
    public static class TaskCommentErrors
    {
        public static Error NotFound(long commentId)
            => Error.NotFound("Comment_NotFound", $"Comment not found with id {commentId}");

       public static Error CreateFailed(long taskId,string commentedBy)
            =>Error.Failure("Comment_CreateFailed", $"Failed to create comment for task {taskId} by user {commentedBy}");

        public static Error DeleteFailed(long commentId, long taskId, string deletedBy)
            => Error.Failure("Comment_DeleteFailed", $"Failed to delete comment {commentId} for task {taskId} by user {deletedBy}");

        public static Error UpdateFailed(long commentId, long taskId, string commentedBy)
            => Error.Failure("Comment_UpdateFailed", $"Failed to update comment {commentId} for task {taskId} by user {commentedBy}");

        public static Error EmptyComment()
            => Error.Validation("Comment_EmptyComment", "Comment text cannot be empty.");

        public static Error CommentTooLong()
            => Error.Validation("Comment_TooLong", "Comment text cannot exceed 2000 characters.");
    }
}
