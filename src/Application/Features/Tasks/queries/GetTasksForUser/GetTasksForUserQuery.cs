using Application.Common.Dtos;
using Application.Features.Tasks.Queries.GetAllProjectTasks;
using ErrorOr;
using MediatR;

namespace Application.Features.Tasks.Queries.GetTasksForUser
{
    public sealed record GetTasksForUserQuery(long WorkSpaceId, long ProjectId, string UserId, PaginationRequestDto PaginationRequestDto, GetAllTasksQueryParameters? FilterParams = null) : IRequest<ErrorOr<PaginationResultDto<TaskDto>>>;
}
