using Application.Common.Dtos;
using ErrorOr;
using MediatR;

namespace Application.Features.Tasks.Queries.GetAllProjectTasks
{
    public sealed record GetAllProjectTasksQuery(long WorkSpaceId, long ProjectId, PaginationRequestDto PaginationRequestDto, GetAllTasksQueryParameters? FilterParams = null) : IRequest<ErrorOr<PaginationResultDto<TaskDto>>>;
}
