using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaces
{
    public record WorkSpaceDto(long Id, string Name, string Description, string CreatedById, 
        DateTime CreatedAt, DateTime? LastUpdatedAt);
}
