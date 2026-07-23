using Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaceInvites.Commands.UpdateWorkSpaceInviteStatus
{
    public sealed record UpdateInviteStatusCommand(long WorkSpaceInviteId, string InviteToId
        ,WorkSpaceInviteStatus WorkSpaceInviteStatus) : IRequest<ErrorOr<bool>>;
    
}
