using Domain.Common.Enums;

namespace Application.Features.Projects.Commands.UpdateProject
{
    public record UpdateProjectDto(string Name, string? Description, ProjectStatus? Status);
}
