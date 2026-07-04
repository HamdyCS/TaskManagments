using Application.Common.Emails;
using Application.Common.Errors;
using Application.Common.Interfaces.Channels;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using ErrorOr;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Users.Commands.RegisterNewUser
{
    public class ConfirmationEmailCommandHandler(IUnitOfWork unitOfWork,
        ILogger<ConfirmationEmailCommandHandler> logger) : IRequestHandler<ConfirmationEmailCommand, ErrorOr<bool>>
    {
        public async Task<ErrorOr<bool>> Handle(ConfirmationEmailCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Start confirmation email {Email}", request.email);



            logger.LogInformation("Get user by email {Email}", request.email);
            var user = await unitOfWork.userRepository.GetByEmailAsync(request.email);
            if (user is null)
            {
                logger.LogWarning("Not found user with email {Email}", request.email);
                return UserErrors.UserNotFoundByEmail(request.email);
            }

            if (user.EmailConfirmed)
            {
                logger.LogWarning("User with email {Email} is already confirmed", request.email);
                return UserErrors.UserAlreadyConfirmed(request.email);
            }


            logger.LogInformation("Confirmation email for user {Email}", request.email);

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.token));
            //confirm email
            var result = await unitOfWork.userRepository.ConfirmUserAsync(user, decodedToken);

            if (!result)
            {
                logger.LogWarning("Failed to Confirmation email {Email}", request.email);
                return UserErrors.ConfirmEmailFailed(request.email);
            }

            return true;
        }
    }
}
