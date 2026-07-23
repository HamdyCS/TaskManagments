using Application.Common.Interfaces.Repositories;
using FluentValidation;

namespace Application.Features.Projects.Commands.CreateProject
{
    public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateProjectCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.CreateProjectDto.Name)
                .NotEmpty().WithMessage("Project name is required")
                .MaximumLength(150).WithMessage("Project name must not exceed 150 characters");

            RuleFor(x => x.CreateProjectDto.Name)
                .MustAsync(async (command, name, CancellationToken) =>
                    await _unitOfWork.ProjectRepository.IsProjectNameUniqueInWorkspaceAsync(command.WorkSpaceId, name))
                .WithMessage("Project name already exists in this workspace");
        }
    }
}
