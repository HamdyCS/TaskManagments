using FluentValidation;

namespace Application.Features.TaskComments.Commands.UpdateComment
{
    public class UpdateCommentCommandValidator : AbstractValidator<UpdateCommentCommand>
    {
        public UpdateCommentCommandValidator()
        {
            RuleFor(x => x.WorkSpaceId)
                .GreaterThan(0).WithMessage("Workspace ID must be greater than 0");

            RuleFor(x => x.ProjectId)
                .GreaterThan(0).WithMessage("Project ID must be greater than 0");

            RuleFor(x => x.TaskId)
                .GreaterThan(0).WithMessage("Task ID must be greater than 0");

            RuleFor(x => x.CommentId)
                .GreaterThan(0).WithMessage("Comment ID must be greater than 0");

            RuleFor(x => x.UpdateCommentDto.Comment)
                .Must(v => !string.IsNullOrWhiteSpace(v?.Trim())).WithMessage("Comment text is required.")
                .MaximumLength(2000).WithMessage("Comment text cannot exceed 2000 characters.");
        }
    }
}
