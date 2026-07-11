using Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces.Services
{
    public interface IWorkSpaceUserService
    {
        Task<bool> IsInWorkSpaceAsync(string userId, long workSpaceId);
        Task<bool> IsUserHasWorkSpaceRoleAsync(string userId, long workSpaceId ,WorkSpaceRole workSpaceRole);
    }
}
