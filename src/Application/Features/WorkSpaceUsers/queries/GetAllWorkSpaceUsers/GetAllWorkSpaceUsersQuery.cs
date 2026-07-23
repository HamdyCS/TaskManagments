using Application.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaceUsers.queries.GetAllWorkSpaceUsers
{
    public sealed record GetAllWorkSpaceUsersQuery(long WorkSpaceId,PaginationRequestDto PaginationRequestDto)
        :IRequest<ErrorOr<PaginationResultDto<WorkSpaceUserDto>>>;
   
}
