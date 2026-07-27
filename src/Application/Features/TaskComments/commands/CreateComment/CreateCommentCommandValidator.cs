using FluentValidation;

namespace Application.Features.TaskComments.Commands.CreateComment
{
    public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
    {
        public CreateCommentCommandValidator()
        {
            RuleFor(x => x.WorkSpaceId)
                .GreaterThan(0).WithMessage("Workspace ID must be greater than 0");

            RuleFor(x => x.ProjectId)
                .GreaterThan(0).WithMessage("Project ID must be greater than 0");

            RuleFor(x => x.TaskId)
                .GreaterThan(0).WithMessage("Task ID must be greater than 0");

            RuleFor(x => x.CreateCommentDto.Comment)
                .Must(v => !string.IsNullOrWhiteSpace(v?.Trim())).WithMessage("Comment text is required.")
                .MaximumLength(2000).WithMessage("Comment text cannot exceed 2000 characters.");
        }
    }
}
