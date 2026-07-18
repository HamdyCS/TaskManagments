using Application.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaceInvites.Queries.GetAllUserInvites
{
    public sealed record GetAllUserInvitesQuery(string InviteToId,PaginationRequestDto PaginationRequest):
        IRequest<ErrorOr<PaginationResultDto<WorkSpaceInviteDto>>>;
    
}
