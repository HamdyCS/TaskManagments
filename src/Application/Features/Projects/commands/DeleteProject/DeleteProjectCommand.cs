using ErrorOr;
using MediatR;

namespace Application.Features.Projects.Commands.DeleteProject
{
    public sealed record DeleteProjectCommand(long WorkSpaceId, long ProjectId, string UserId) : IRequest<ErrorOr<Success>>;
}
