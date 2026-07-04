using Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces.Services
{
    public interface ITokenService
    {
        string GenerateToken(string userId, string email,Role role);
        string GenerateRefreshToken();
    }
}
