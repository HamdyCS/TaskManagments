using Application.Features.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaces
{
    public record WorkSpaceDto(long Id, string Name, string Description, string CreatedById, string CreatedByName, 
        DateTime CreatedAt, string LastUpdatedById,DateTime? LastUpdatedAt);
}
