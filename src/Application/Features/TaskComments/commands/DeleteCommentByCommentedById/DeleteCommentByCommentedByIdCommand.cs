using ErrorOr;
using MediatR;

namespace Application.Features.TaskComments.Commands.DeleteCommentByCommentedById
{
    public sealed record DeleteCommentByCommentedByIdCommand(
        long WorkSpaceId,
        long ProjectId,
        long TaskId,
        long CommentId,
        string CommentedById,
        bool IsAdminOrOwner) : IRequest<ErrorOr<Deleted>>;
}
