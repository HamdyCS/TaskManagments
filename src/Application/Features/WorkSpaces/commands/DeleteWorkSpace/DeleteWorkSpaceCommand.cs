using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaces.commands.DeleteWorkSpace
{
    public sealed record DeleteWorkSpaceCommand(long WorkSpaceId ,string DeleteBy) : IRequest<ErrorOr<bool>>;
    
}
