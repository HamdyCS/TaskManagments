using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.ChangeEmail
{
    public class ChangeEmailCommandHandler(IUnitOfWork unitOfWork, ILogger<ChangeEmailCommandHandler> logger)
        : IRequestHandler<ChangeEmailCommand, ErrorOr<bool>>
    {
        public async Task<ErrorOr<bool>> Handle(ChangeEmailCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting Change Email for user with Id {UserId}", request.UserId);

            //get user
            logger.LogInformation("Getting user By Id {UserId}", request.UserId);

            var user = await unitOfWork.userRepository.GetByIdAsync(request.UserId);

            if (user is null)
            {
                logger.LogInformation("User with Id {UserId} not found", request.UserId);
                return UserErrors.UserNotFoundById(request.UserId);
            }

            //check if email exists
            logger.LogInformation("Checking if email {NewEmail} exists", request.ChangeEmailDto.NewEmail);
            var emailExists = await unitOfWork.userRepository.IsExistByEmailAsync(request.ChangeEmailDto.NewEmail);

            if (emailExists)
            {
                logger.LogWarning("Email {NewEmail} already exists", request.ChangeEmailDto.NewEmail);
                return UserErrors.EmailAlreadyExist(request.ChangeEmailDto.NewEmail);
            }

            //change email
            logger.LogInformation("Changing email for user with Id {UserId}", request.UserId);

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.ChangeEmailDto.Token));

            var result = await unitOfWork.userRepository.ChangeEmailAsync(user, decodedToken, request.ChangeEmailDto.NewEmail);

            if(!result)
            {
                logger.LogWarning("Failed to change email for user with Id {UserId}", request.UserId);
                return UserErrors.ChangeEmailFailed(request.UserId);
            }

            logger.LogInformation("Email Changed for user with Id {UserId} successfully", request.UserId);
            return true;
        }
    }
}
