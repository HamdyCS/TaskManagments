using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaceInvites.Commands.CreateWorkSpaceInvite
{
    public sealed record CreateInviteCommand(CreateInviteDto CreateWorkSpaceInviteDto,
        string UserId) : IRequest<ErrorOr<WorkSpaceInviteDto>>;
    
}
