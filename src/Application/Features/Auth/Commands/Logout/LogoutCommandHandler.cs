
using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.Logout
{
    public class LogoutCommandHandler(IUnitOfWork unitOfWork, ILogger<LogoutCommandHandler> logger) : IRequestHandler<LogoutCommand, ErrorOr<bool>>
    {
        public async Task<ErrorOr<bool>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting logout of user with Id {UserId}", request.userId);

            var refreshToken = await unitOfWork.RefreshTokenRepository.GetByTokenAsync(request.refreshToken);
            if (refreshToken is null)
            {
                logger.LogWarning("Refresh token not found for user with Id {UserId}", request.userId);
                return RefreshTokenErrors.RefreshTokenNotFound;
            }

            refreshToken.IsRevoked = true;
            refreshToken.RevokedAt = DateTime.UtcNow;

            logger.LogInformation("Refresh token revoked for user with Id {UserId}", request.userId);
            unitOfWork.RefreshTokenRepository.Update(refreshToken);

            var rowsAffected = await unitOfWork.SaveChangesAsync(cancellationToken);
            if (rowsAffected == 0)
            {
                logger.LogWarning("Refresh token not Revoked for user with Id {UserId}", request.userId);
                return RefreshTokenErrors.RefreshTokenNotRevoked(refreshToken.Id);
            }
            logger.LogInformation("Refresh token revoked for user with Id {UserId}", request.userId);


            logger.LogInformation("Logout completed for user with Id {UserId}", request.userId);
            return true;
        }
    }
}
