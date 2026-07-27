using ErrorOr;
using MediatR;

namespace Application.Features.TaskComments.Queries.GetCommentById
{
    public sealed record GetCommentByIdQuery(
        long WorkSpaceId,
        long ProjectId,
        long TaskId,
        long CommentId) : IRequest<ErrorOr<TaskCommentDto>>;
}
