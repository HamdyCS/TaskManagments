using Application.Common.Interfaces.Services;
using Domain.Entities;
using Mapster;

namespace Application.Features.TaskAttachments
{
    public class TaskAttachmentDtoMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<TaskAttachment, TaskAttachmentDto>();
        }
    }
}
