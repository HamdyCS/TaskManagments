using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaces.commands.UpdateWorkSpace
{
    public sealed record UpdateWorkSpaceCommand(UpdateWorkSpaceDto UpdateWorkSpaceDto,long WorkSpaceId ,string UpdateBy) : IRequest<ErrorOr<bool>>;
    
}
