using ErrorOr;
using MediatR;

namespace Application.Features.TaskComments.Commands.UpdateComment
{
    public sealed record UpdateCommentCommand(
        UpdateCommentDto UpdateCommentDto,
        long WorkSpaceId,
        long ProjectId,
        long TaskId,
        long CommentId,
        string CommentedById) : IRequest<ErrorOr<TaskCommentDto>>;
}
