using Application.Common.Emails;
using Application.Common.Errors;
using Application.Common.Interfaces.Channels;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using ErrorOr;
using Mapster;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Users.Commands.RegisterNewUser
{
    public class RegisterUserCommandHandler(IUnitOfWork unitOfWork, IConfiguration configuration, 
        ILogger<ConfirmationEmailCommandHandler> logger,IConfirmationEmailQueue confirmationEmailQueue) : IRequestHandler<RegisterUserCommand, ErrorOr<RegisterUserResultDto>>
    {
        public async Task<ErrorOr<RegisterUserResultDto>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Start Registering new user with email {Email}", request.registerNewUserDto.Email);

            var registerNewUserDto = request.registerNewUserDto;
            var role = request.enRole;

            //check if email is already in use
            logger.LogInformation("Checking if email {Email} is already in use", registerNewUserDto.Email);
            var isEmailExist = await unitOfWork.userRepository.IsExistByEmailAsync(registerNewUserDto.Email);
            if (isEmailExist)
            {
                logger.LogWarning("Email {Email} is already in use", registerNewUserDto.Email);
                return UserErrors.EmailAlreadyExist(registerNewUserDto.Email);
            }

            //mapping registerNewUserDto to User
            var user = registerNewUserDto.Adapt<User>();
            user.CreatedAt = DateTime.UtcNow;
            user.RoleId = (short)role;


            logger.LogInformation("Adding new user with email {Email} to database", registerNewUserDto.Email);

            //add new user
            var result = await unitOfWork.userRepository.AddNewUserAsync(user);

            if ( result.token is null)
            {
                logger.LogWarning("Failed to register new user. token is null");
                return UserErrors.RegisterFailed;
            }

            var path = configuration["settings:frontendUrl"] + "?email=" + result.email + "&token=" + result.token;
            logger.LogInformation("User registered successfully email {email}", result.email);

            //add email to queue
            await confirmationEmailQueue.EnqueueAsync(new ConfirmationEmailContent
            {
                Subject = "Please confirm your email",
                TextBody = "Your Confirmation link is: " + path,
                Messsage = "Please confirm your email by clicking the Confirmation Button",
                To = result.email,
                Url = path,
                FullName = user.FullName
            });

            //return RegisterUserResultDto
            return new RegisterUserResultDto { Id = user.Id };
        }
    }
}
