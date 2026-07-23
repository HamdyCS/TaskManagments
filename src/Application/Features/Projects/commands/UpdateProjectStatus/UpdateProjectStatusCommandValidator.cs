using Domain.Common.Enums;
using FluentValidation;

namespace Application.Features.Projects.Commands.UpdateProjectStatus
{
    public class UpdateProjectStatusCommandValidator : AbstractValidator<UpdateProjectStatusCommand>
    {
        public UpdateProjectStatusCommandValidator()
        {
            RuleFor(x => x.UpdateProjectStatusDto.Status)
                .IsInEnum().WithMessage("Invalid status value");
        }
    }
}
