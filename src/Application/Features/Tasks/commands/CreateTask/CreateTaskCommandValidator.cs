using FluentValidation;

namespace Application.Features.Tasks.Commands.CreateTask
{
    public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
    {
        public CreateTaskCommandValidator()
        {
            RuleFor(x => x.WorkSpaceId)
                .GreaterThan(0).WithMessage("Workspace ID must be greater than 0");

            RuleFor(x => x.CreateTaskDto.Name)
                .NotEmpty().WithMessage("Task name is required")
                .MaximumLength(200).WithMessage("Task name must not exceed 200 characters");

            RuleFor(x => x.CreateTaskDto.Description)
                .MaximumLength(2000).WithMessage("Task description must not exceed 2000 characters");

            RuleFor(x => x.CreateTaskDto.Priority)
                .IsInEnum().WithMessage("Invalid task priority");

            RuleFor(x => x.CreateTaskDto.Deadline)
                .Must(deadline => !deadline.HasValue || deadline.Value > DateTime.UtcNow)
                .WithMessage("Deadline must be in the future");
        }
    }
}
