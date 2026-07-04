using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Domain.Common.Enums;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.CreateToken
{
    public class CreateTokenCommandHandler(ITokenService tokenService, IUnitOfWork unitOfWork, ILogger<CreateTokenCommandHandler> logger) : IRequestHandler<CreateTokenCommand, ErrorOr<string>>
    {
        public async Task<ErrorOr<string>> Handle(CreateTokenCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting Create Token for user with id {userId}", request.userId);

            //get refresh token

            logger.LogInformation("Getting refresh token from db for user with id {userId}", request.userId);
            var refreshToken = await unitOfWork.refreshTokenRepository.GetByTokenAsync(request.refreshToken);

            if(refreshToken is null)
            {
                logger.LogInformation("Refresh token not found for user with id {userId}", request.userId);
                return RefreshTokenErrors.RefreshTokenNotFound;
            }

            if(refreshToken.IsRevoked || refreshToken.IsExpired)
            {
                logger.LogInformation("Refresh token is revoked or expired for user with id {userId}", request.userId);
                return RefreshTokenErrors.RefreshTokenRevokedOrExpired(refreshToken.Id);
            }

            //get user
            logger.LogInformation("Getting user with Id {UserId} from db", refreshToken.UserId);
            var user = await unitOfWork.userRepository.GetByIdAsync(refreshToken.UserId);
            if (user is null)
            {
                logger.LogInformation("User with Id {UserId} not found", refreshToken.UserId);
                return UserErrors.UserNotFoundById(refreshToken.UserId);
            }


            //create new access token
            var accessToken = tokenService.GenerateToken(refreshToken.UserId, user.Email,(Role)user.RoleId);
            if(accessToken is null)
            {
                logger.LogInformation("Access token could not be created to user with Id {UserId}", refreshToken.UserId);
                return TokenErrors.CreatedFailed(refreshToken.UserId);
            }

            logger.LogInformation("Created access token successfully for user with Id {UserId}", refreshToken.UserId);
            return accessToken;
        }
    }
}
