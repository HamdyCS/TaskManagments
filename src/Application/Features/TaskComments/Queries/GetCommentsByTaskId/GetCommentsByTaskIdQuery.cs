using Application.Common.Dtos;
using ErrorOr;
using MediatR;

namespace Application.Features.TaskComments.Queries.GetCommentsByTaskId
{
    public sealed record GetCommentsByTaskIdQuery(
        long WorkSpaceId,
        long ProjectId,
        long TaskId,
        int PageNumber = 1,
        int PageSize = 10) : IRequest<ErrorOr<PaginationResultDto<TaskCommentDto>>>;
}
