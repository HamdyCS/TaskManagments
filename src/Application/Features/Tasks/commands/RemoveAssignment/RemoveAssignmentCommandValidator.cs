using FluentValidation;

namespace Application.Features.Tasks.Commands.RemoveAssignment
{
    public class RemoveAssignmentCommandValidator : AbstractValidator<RemoveAssignmentCommand>
    {
        public RemoveAssignmentCommandValidator()
        {
            RuleFor(x => x.WorkSpaceId)
                .GreaterThan(0).WithMessage("Workspace ID must be greater than 0");

            RuleFor(x => x.ProjectId)
                .GreaterThan(0).WithMessage("Project ID must be greater than 0");

            RuleFor(x => x.TaskId)
                .GreaterThan(0).WithMessage("Task ID must be greater than 0");

            RuleFor(x => x.AssignedUserId)
                .NotEmpty().WithMessage("Assigned user ID is required");
        }
    }
}
