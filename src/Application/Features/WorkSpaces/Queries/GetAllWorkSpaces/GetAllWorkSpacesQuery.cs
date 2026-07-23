using Application.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaces.commands.GetAllWorkSpaces
{
    public sealed record GetAllWorkSpacesQuery(PaginationRequestDto PaginationRequestDto) : IRequest<ErrorOr<PaginationResultDto<WorkSpaceDto>>>;
    
}
