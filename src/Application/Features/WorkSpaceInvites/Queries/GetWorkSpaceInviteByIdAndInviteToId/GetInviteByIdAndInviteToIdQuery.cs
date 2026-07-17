using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaceInvites.Queries.GetInviteByIdAndInviteToId
{
    public sealed record GetInviteByIdAndInviteToIdQuery(long WorkSpaceInviteId,string InviteToId):IRequest<ErrorOr<WorkSpaceInviteDto>>;
    
}
