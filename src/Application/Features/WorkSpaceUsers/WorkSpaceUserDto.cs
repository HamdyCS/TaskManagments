using Application.Features.WorkSpaces;
using Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaceUsers
{
    public record WorkSpaceUserDto(long Id,string UserId,string FullName,string Email,WorkSpaceRole WorkSpaceRole,WorkSpaceDto WorkSpaceDto);
   
}
