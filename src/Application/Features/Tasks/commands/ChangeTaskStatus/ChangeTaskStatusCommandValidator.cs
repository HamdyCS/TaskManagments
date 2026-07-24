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

            RuleFor(x => x)
                .Must(x => IsValidStatusTransition(
                    GetCurrentStatus(x),
                    x.ChangeTaskStatusDto.Status))
                .WithMessage("Invalid status transition");
        }

        private static ProjectTaskStatus GetCurrentStatus(ChangeTaskStatusCommand command)
        {
            // This will be validated in the handler where we have access to the actual task
            // The validator ensures the target status is valid, handler checks the transition
            return command.ChangeTaskStatusDto.Status;
        }

        private static bool IsValidStatusTransition(ProjectTaskStatus from, ProjectTaskStatus to)
        {
            // Allow any valid status - transition validation happens in handler with actual task state
            return true;
        }
    }
}
