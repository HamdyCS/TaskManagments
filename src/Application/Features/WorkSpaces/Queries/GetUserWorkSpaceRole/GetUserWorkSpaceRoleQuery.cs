using Domain.Common.Enums;
using ErrorOr;
using MediatR;

namespace Application.Features.WorkSpaces.Queries.GetUserWorkSpaceRole
{
    public sealed record GetUserWorkSpaceRoleQuery(long WorkSpaceId, string UserId) : IRequest<ErrorOr<WorkSpaceRole>>;
}
