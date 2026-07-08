using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Application.Features.Auth.Commands.VerfiyOtp;
using Domain.Common.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.ForgetPassword
{
    public class ForgetPasswordCommandHandler(IUnitOfWork unitOfWork,IMediator mediator
        ,ILogger<ForgetPasswordCommandHandler> logger) : IRequestHandler<ForgetPasswordCommand, ErrorOr<bool>>
    {
        public async Task<ErrorOr<bool>> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting forget password for user with email {Email}", request.ForgetPasswordDto.Email);


            var user = await unitOfWork.userRepository.GetByEmailAsync(request.ForgetPasswordDto.Email);
            if(user is null)
            {
                logger.LogWarning("User with email {Email} not found", request.ForgetPasswordDto.Email);
                return UserErrors.UserNotFoundByEmail(request.ForgetPasswordDto.Email);
            }

            //verify otp

            logger.LogInformation("Verifying Otp for user with email {Email}", request.ForgetPasswordDto.Email);
            var verifyOtpResult =await
                mediator.Send(new VerifyOtpCommand(new VerifyOtpDto
                {
                    Email = request.ForgetPasswordDto.Email,
                    Otp = request.ForgetPasswordDto.Otp
                }, OtpPurpose.ForgetPassword),cancellationToken);


            if(verifyOtpResult.IsError)
            {
                logger.LogWarning("Failed to verify otp for user with email {Email}", request.ForgetPasswordDto.Email);
                return verifyOtpResult.Errors;
            }

            //update password

            logger.LogInformation("Updating password for user with email {Email}", request.ForgetPasswordDto.Email);
            var isPasswordUpdated = await unitOfWork.userRepository.UpdatePasswordAsync(user, request.ForgetPasswordDto.NewPassword);

            if(!isPasswordUpdated)
            {
                logger.LogError("Failed to update password for user with email {Email}", request.ForgetPasswordDto.Email);
                return UserErrors.UpdatedPasswordFailedByEmail(request.ForgetPasswordDto.Email);
            }

            logger.LogInformation("Password updated for user with email {Email} successfully", request.ForgetPasswordDto.Email);

            return true;
        }
    }
}
