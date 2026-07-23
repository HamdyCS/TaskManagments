using Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaceInvites.Commands.CreateWorkSpaceInvite
{
    public record CreateInviteDto(long WorkSpaceId, string InviteToEmail, WorkSpaceRole WorkSpaceRole);
}
