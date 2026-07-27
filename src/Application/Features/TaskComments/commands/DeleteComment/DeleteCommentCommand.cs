using ErrorOr;
using MediatR;

namespace Application.Features.TaskComments.Commands.DeleteComment
{
    public sealed record DeleteCommentCommand(
        long WorkSpaceId,
        long ProjectId,
        long TaskId,
        long CommentId,
        string UserId,
        bool IsAdminOrOwner) : IRequest<ErrorOr<Deleted>>;
}
