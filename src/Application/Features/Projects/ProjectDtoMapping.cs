using Domain.Entities;
using Mapster;

namespace Application.Features.Projects
{
    public class ProjectDtoMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // Project to ProjectDto
            config.NewConfig<Project, ProjectDto>();
        }
    }
}
