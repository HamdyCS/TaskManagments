using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler(IUnitOfWork unitOfWork, ILogger<ResetPasswordCommandHandler> logger)
        : IRequestHandler<ResetPasswordCommand, ErrorOr<bool>>
    {
        public async Task<ErrorOr<bool>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting Reset password for user with Id {UserId}", request.UserId);

            //get user
            logger.LogInformation("Getting user By Id {UserId}", request.UserId);

            var user = await unitOfWork.UserRepository.GetByIdAsync(request.UserId);

            if (user is null)
            {
                logger.LogInformation("User with Id {UserId} not found", request.UserId);
                return UserErrors.UserNotFoundById(request.UserId);
            }

            //reset password
            logger.LogInformation("Resetting password for user with Id {UserId}", request.UserId);

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.ResetPasswordDto.Token));

            var result = await unitOfWork.UserRepository.ResetPasswordAsync(user, decodedToken, request.ResetPasswordDto.NewPassword);

            if(!result)
            {
                logger.LogWarning("Failed to reset password for user with Id {UserId}", request.UserId);
                return UserErrors.ResetPasswordFailed(request.UserId);
            }

            logger.LogInformation("Password reseted for user with Id {UserId} successfully", request.UserId);
            return true;
        }
    }
}
