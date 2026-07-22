using Application.Common.Interfaces.Repositories;
using Domain.Common.Enums;
using FluentValidation;

namespace Application.Features.Projects.Commands.UpdateProject
{
    public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
    {

        public UpdateProjectCommandValidator(IUnitOfWork unitOfWork)
        {

            RuleFor(x => x.UpdateProjectDto.Name)
                .NotEmpty().WithMessage("Project name is required")
                .MaximumLength(150).WithMessage("Project name must not exceed 150 characters");

            RuleFor(x => x.UpdateProjectDto.Name)
                .MustAsync(async (command, name, CancellationToken) =>
                    await unitOfWork.ProjectRepository.IsProjectNameUniqueInWorkspaceAsync(command.WorkSpaceId, name, command.ProjectId))
                .WithMessage("Project name already exists in this workspace");

            RuleFor(x => x.UpdateProjectDto.Status)
                .Must(status => status == null || Enum.IsDefined(typeof(ProjectStatus), status.Value))
                .WithMessage("Invalid status value");
        }
    }
}
