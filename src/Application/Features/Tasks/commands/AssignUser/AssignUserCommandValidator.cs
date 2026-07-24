using FluentValidation;

namespace Application.Features.Tasks.Commands.AssignUsers
{
    public class AssignUserCommandValidator : AbstractValidator<AssignUserCommand>
    {
        public AssignUserCommandValidator()
        {
            RuleFor(x => x.WorkSpaceId)
                .GreaterThan(0).WithMessage("Workspace ID must be greater than 0");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required");
        }
    }
}
