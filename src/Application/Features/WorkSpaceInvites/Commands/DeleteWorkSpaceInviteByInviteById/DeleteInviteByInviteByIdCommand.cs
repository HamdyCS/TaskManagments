using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaceInvites.Commands.DeleteWorkSpaceInviteByInviteById
{
    public sealed record DeleteInviteByInviteByIdCommand(long WorkSpaceInviteId, string InviteById) : IRequest<ErrorOr<bool>>;

}
