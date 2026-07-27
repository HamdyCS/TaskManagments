using ErrorOr;
using MediatR;

namespace Application.Features.TaskComments.Commands.CreateComment
{
    public sealed record CreateCommentCommand(
        CreateCommentDto CreateCommentDto,
        long WorkSpaceId,
        long ProjectId,
        long TaskId,
        string UserId) : IRequest<ErrorOr<TaskCommentDto>>;
}
