using Application.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaces.commands.GetWorkSpaceById
{
    public sealed record GetWorkSpaceByIdQuery(long WorkSpaceId) : IRequest<ErrorOr<WorkSpaceDto>>;
    
}
