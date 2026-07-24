using Domain.Common.Enums;

namespace Application.Features.Tasks.Commands.ChangeTaskStatus
{
    public record ChangeTaskStatusDto(ProjectTaskStatus Status);
}
