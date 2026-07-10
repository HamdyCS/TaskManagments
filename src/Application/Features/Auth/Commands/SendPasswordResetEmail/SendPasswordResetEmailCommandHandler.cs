using Application.Common.Emails;
using Application.Common.Errors;
using Application.Common.Interfaces.Channels;
using Application.Common.Interfaces.Repositories;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.SendPasswordResetEmail
{
    public class SendPasswordResetEmailCommandHandler(IUnitOfWork unitOfWork,
        IResetPasswordEmailQueue resetPasswordEmailQueue, IConfiguration configuration,
        ILogger<SendPasswordResetEmailCommandHandler> logger) : IRequestHandler<SendPasswordResetEmailCommand, ErrorOr<bool>>
    {
        public async Task<ErrorOr<bool>> Handle(SendPasswordResetEmailCommand request, CancellationToken cancellationToken)
        {
           logger.LogInformation("Started sending password reset email for user with id {UserId}", request.UserId);

            //get user
            logger.LogInformation("Getting user with id {UserId}", request.UserId);
            var user = await unitOfWork.UserRepository.GetByIdAsync(request.UserId);

            if(user is null)
            {
                logger.LogWarning("User with id {UserId} not found", request.UserId);
                return UserErrors.UserNotFoundById(request.UserId);
            }

            //generate password reset token
            logger.LogInformation("Generating password reset token for user with id {UserId}", request.UserId);
            var token =await unitOfWork.UserRepository.GeneratePasswordResetTokenAsync(user);

            if(token is null)
            {
                logger.LogWarning("Password reset token not generated for user with id {UserId}", request.UserId);
                return UserErrors.ResetPasswordFailed(request.UserId);
            }

            var encodedToken = WebEncoders.Base64UrlEncode(System.Text.Encoding.UTF8.GetBytes(token));
            var path = configuration["settings:frontendUrl"] + "/reset-password?token=" + encodedToken;

            //send email
            logger.LogInformation("Adding password reset email to queue for user with id {UserId}", request.UserId);
            await resetPasswordEmailQueue.EnqueueAsync(new ResetPasswordEmailContent
            {
                FullName = user.FullName,
                To = user.Email,
                Url = path
            });

            logger.LogInformation("Password reset email sent for user with id {UserId} successfully", request.UserId);
            return true;
        }
    }
}
