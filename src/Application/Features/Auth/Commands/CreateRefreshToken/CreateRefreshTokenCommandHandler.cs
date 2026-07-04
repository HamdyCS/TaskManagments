using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Domain.Entities;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.CreateRefreshToken
{
    public class CreateRefreshTokenCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateRefreshTokenCommandHandler> logger, ITokenService tokenService, IConfiguration configuration) : IRequestHandler<CreateRefreshTokenCommand, ErrorOr<string>>
    {
        public async Task<ErrorOr<string>> Handle(CreateRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting Create Refresh Token to user with Id {UserId}", request.userId);

            var user = await unitOfWork.userRepository.GetByIdAsync(request.userId);
            if (user is null)
                return UserErrors.UserNotFoundById(request.userId);

            //get active refresh token

            logger.LogInformation("Get active refresh token of user with Id {UserId}", request.userId);
            var activeRefreshToken = await unitOfWork.refreshTokenRepository.GetActiveRefreshTokenAsync(request.userId);
            if (activeRefreshToken is not null)
            {
                activeRefreshToken.IsRevoked = true;
                unitOfWork.refreshTokenRepository.Update(activeRefreshToken);

                //change active refresh token to revoked

                logger.LogInformation("Change active refresh token with Id {RefreshTokenId} of UserId {UserId} to revoked", activeRefreshToken.Id, request.userId);
                var isRevoked = await unitOfWork.SaveChangesAsync(cancellationToken) > 0;

                if (!isRevoked)
                {
                    logger.LogWarning("Failed to change active refresh token with Id {RefreshTokenId} of UserId {UserId} to revoked", activeRefreshToken.Id, request.userId);
                    return RefreshTokenErrors.FailedRevokingRefreshToken(activeRefreshToken.Id);
                }
            }

            //get refresh token life time
            var lifeTimeDays = int.TryParse(configuration["RefreshToken:LifeTimeDays"], out int outResult) ? outResult : 0;


            //create new refresh token
            RefreshToken newRefreshToken = new RefreshToken
            {
                IsRevoked = false,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(lifeTimeDays),
                Token = tokenService.GenerateRefreshToken(),
                UserId = request.userId
            };

            //save to db
            logger.LogInformation("Add new refresh token of user with Id {UserId} to db", request.userId);
            unitOfWork.refreshTokenRepository.Add(newRefreshToken);
            var isNewRefreshTokenSaved = await unitOfWork.SaveChangesAsync(cancellationToken) > 0;

            if (!isNewRefreshTokenSaved)
            {
                logger.LogWarning("Failed to Add new refresh to user with Id {UserId} token to db", request.userId);
                return RefreshTokenErrors.CreatedFailed(request.userId);
            }

            logger.LogInformation("Added new refresh token successfully of user with Id {UserId}", request.userId);

            return newRefreshToken.Token;
        }
    }
}
