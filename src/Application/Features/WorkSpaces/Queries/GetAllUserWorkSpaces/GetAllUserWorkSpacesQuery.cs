using Application.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaces.commands.GetAllUserWorkSpaces
{
    public sealed record GetAllUserWorkSpacesQuery(string UserId,PaginationRequestDto PaginationRequestDto) : IRequest<ErrorOr<PaginationResultDto<WorkSpaceDto>>>;
    
}
