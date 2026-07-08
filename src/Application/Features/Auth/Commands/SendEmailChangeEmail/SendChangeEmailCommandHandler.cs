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

namespace Application.Features.Auth.Commands.SendEmailChangeEmail
{
    public class SendChangeEmailCommandHandler(IUnitOfWork unitOfWork,
        IChangeEmailQueue changeEmailQueue, IConfiguration configuration,
        ILogger<SendChangeEmailCommandHandler> logger) : IRequestHandler<SendChangeEmailCommand, ErrorOr<bool>>
    {
        public async Task<ErrorOr<bool>> Handle(SendChangeEmailCommand request, CancellationToken cancellationToken)
        {
           logger.LogInformation("Started sending change email for user with id {UserId}", request.UserId);

            //get user
            logger.LogInformation("Getting user with id {UserId}", request.UserId);
            var user = await unitOfWork.userRepository.GetByIdAsync(request.UserId);

            if(user is null)
            {
                logger.LogWarning("User with id {UserId} not found", request.UserId);
                return UserErrors.UserNotFoundById(request.UserId);
            }

            //check if email exists
            logger.LogInformation("Checking if email {NewEmail} exists", request.NewEmail);
            var emailExists = await unitOfWork.userRepository.IsExistByEmailAsync(request.NewEmail);

            if(emailExists)
            {
                logger.LogWarning("Email {NewEmail} already exists", request.NewEmail);
                return UserErrors.EmailAlreadyExist(request.NewEmail);
            }

            //generate change email token
            logger.LogInformation("Generating change email token for user with id {UserId}", request.UserId);
            var token =await unitOfWork.userRepository.GenerateChangeEmailTokenAsync(user,request.NewEmail);

            if(token is null)
            {
                logger.LogWarning("Change email token not generated for user with id {UserId}", request.UserId);
                return UserErrors.ChangeEmailFailed(request.UserId);
            }

            var encodedToken = WebEncoders.Base64UrlEncode(System.Text.Encoding.UTF8.GetBytes(token));
            var path = configuration["settings:frontendUrl"] + "/change-email?token=" + encodedToken;

            //send email
            logger.LogInformation("Adding change email to queue for user with id {UserId}", request.UserId);
            await changeEmailQueue.EnqueueAsync(new ChangeEmailContent
            {
                FullName = user.FullName,
                To = user.Email,
                Url = path
            });

            logger.LogInformation("Change email sent for user with id {UserId} successfully", request.UserId);
            return true;
        }
    }
}
