using Domain.Common.Enums;
using FluentValidation;

namespace Application.Features.Tasks.Commands.ChangeTaskStatus
{
    public class ChangeTaskStatusCommandValidator : AbstractValidator<ChangeTaskStatusCommand>
    {
        public ChangeTaskStatusCommandValidator()
        {
            RuleFor(x => x.WorkSpaceId)
                .GreaterThan(0).WithMessage("Workspace ID must be greater than 0");

            RuleFor(x => x.ChangeTaskStatusDto.Status)
                .IsInEnum().WithMessage("Invalid task status");

           RuleFor(x => x.ProjectId)
                .GreaterThan(0).WithMessage("ProjectId must be greater than 0");

            RuleFor(x => x.TaskId)
                .GreaterThan(0).WithMessage("TaskId must be greater than 0");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required");
        }

      
    }
}
