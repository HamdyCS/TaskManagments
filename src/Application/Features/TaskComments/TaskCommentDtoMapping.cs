using Domain.Entities;
using Mapster;

namespace Application.Features.TaskComments
{
    public class TaskCommentDtoMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<TaskComment, TaskCommentDto>()
                .Map(dest => dest.CommentByName, src => src.CommentByName);
                
        }
    }
}
