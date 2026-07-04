using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(AppDbContext context) : base(context)
        {
            
        }
        public async Task<RefreshToken?> GetActiveRefreshTokenAsync(string userId)
        {
            var activeRefreshToken = await GetByFilterAsync(rt =>
            rt.UserId == userId && !rt.IsRevoked && rt.ExpiresAt > DateTime.UtcNow);

            return activeRefreshToken;
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            var refreshToken = await GetByFilterAsync(rt => rt.Token == token);
            return refreshToken;
        }

        public async Task<bool> IsActiveRefreshTokenAsync(string refreshToken)
        {
            var isActive = await context.RefreshTokens.
                AnyAsync(rt => rt.Token == refreshToken && !rt.IsRevoked && rt.ExpiresAt > DateTime.UtcNow);
            return isActive;
        }
    }
}
