using FluentValidation;

namespace Application.Features.TaskComments.Commands.DeleteComment
{
    public class DeleteCommentCommandValidator : AbstractValidator<DeleteCommentCommand>
    {
        public DeleteCommentCommandValidator()
        {
            RuleFor(x => x.WorkSpaceId)
                .GreaterThan(0).WithMessage("Workspace ID must be greater than 0");

            RuleFor(x => x.ProjectId)
                .GreaterThan(0).WithMessage("Project ID must be greater than 0");

            RuleFor(x => x.TaskId)
                .GreaterThan(0).WithMessage("Task ID must be greater than 0");

            RuleFor(x => x.CommentId)
                .GreaterThan(0).WithMessage("Comment ID must be greater than 0");
        }
    }
}
