using Application.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaceInvites.Queries.GetAllSendWorkSpaceInvites
{
    public sealed record GetAllSendInvitesQuery(string InviteById,PaginationRequestDto PaginationRequest):
        IRequest<ErrorOr<PaginationResultDto<WorkSpaceInviteDto>>>;
    
}
