using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaceInvites.Queries.GetWorkSpaceInviteById
{
    public sealed record GetInviteByIdQuery(long WorkSpaceInviteId):IRequest<ErrorOr<WorkSpaceInviteDto>>;
    
}
