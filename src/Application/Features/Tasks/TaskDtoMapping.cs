using Domain.Entities;
using Mapster;

namespace Application.Features.Tasks
{
    public class TaskDtoMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<ProjectTask, TaskDto>()
                .Map(dest => dest.Assignments, src => src.TaskAssignments)
                .Map(dest => dest.Attachments, src => src.TaskAttachments);

            config.NewConfig<TaskAssignment, TaskAssignmentDto>();
        }
    }
}
