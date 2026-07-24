using Application.Common.Dtos;
using Application.Features.Tasks.Queries.GetAllProjectTasks;
using ErrorOr;
using MediatR;

namespace Application.Features.Tasks.Queries.GetMyTasks
{
    public sealed record GetMyTasksQuery(long WorkSpaceId, long ProjectId, string UserId, PaginationRequestDto PaginationRequestDto, GetAllTasksQueryParameters? FilterParams = null) : IRequest<ErrorOr<PaginationResultDto<TaskDto>>>;
}
