using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaces.commands.CreateWorkSpace
{
    public sealed record CreateWorkSpaceCommand(CreateWorkSpaceDto CreateWorkSpaceDto, string CreateBy) : IRequest<ErrorOr<WorkSpaceDto>>;
    
}
