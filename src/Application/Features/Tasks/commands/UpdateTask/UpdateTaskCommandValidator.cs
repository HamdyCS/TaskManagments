using FluentValidation;

namespace Application.Features.Tasks.Commands.UpdateTask
{
    public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
    {
        public UpdateTaskCommandValidator()
        {
            RuleFor(x => x.WorkSpaceId)
                .GreaterThan(0).WithMessage("Workspace ID must be greater than 0");

            RuleFor(x => x.UpdateTaskDto.Name)
                .MaximumLength(200).WithMessage("Task name must not exceed 200 characters")
                .When(x => x.UpdateTaskDto.Name is not null);

            RuleFor(x => x.UpdateTaskDto.Description)
                .MaximumLength(2000).WithMessage("Task description must not exceed 2000 characters")
                .When(x => x.UpdateTaskDto.Description is not null);

            RuleFor(x => x.UpdateTaskDto.Priority)
                .IsInEnum().WithMessage("Invalid task priority")
                .When(x => x.UpdateTaskDto.Priority.HasValue);

            RuleFor(x => x.UpdateTaskDto.Deadline)
                .Must(deadline => !deadline.HasValue || deadline.Value > DateTime.UtcNow)
                .WithMessage("Deadline must be in the future")
                .When(x => x.UpdateTaskDto.Deadline.HasValue);
        }
    }
}
