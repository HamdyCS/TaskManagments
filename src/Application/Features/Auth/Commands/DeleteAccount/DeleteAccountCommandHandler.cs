using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Application.Features.Auth.Commands.VerifyOtp;
using Domain.Common.Enums;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.DeleteAccount
{
    public class DeleteAccountCommandHandler(IUnitOfWork unitOfWork, IMediator mediator
        , ILogger<DeleteAccountCommandHandler> logger) : IRequestHandler<DeleteAccountCommand, ErrorOr<bool>>
    {
        public async Task<ErrorOr<bool>> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting Delete User with Id {UserId}", request.UserId);


            logger.LogInformation("Getting user with Id {UserId}", request.UserId);
            var user = await unitOfWork.userRepository.GetByIdAsync(request.UserId);
            if (user is null)
            {
                logger.LogWarning("User with Id {UserId} not found", request.UserId);
                return UserErrors.UserNotFoundById(request.UserId);
            }

            //verify otp

            logger.LogInformation("Verifying Otp for user with Id {UserId}", request.UserId);

            var VerifyOtpDto = request.DeleteAccountDto.Adapt<VerifyOtpDto>();
            var verifyOtpResult = await
                mediator.Send(new VerifyOtpCommand(VerifyOtpDto, OtpPurpose.DeleteAccount), cancellationToken);


            if (verifyOtpResult.IsError)
            {
                logger.LogWarning("Failed to verify otp for user with Id {UserId}", request.UserId);
                return verifyOtpResult.Errors;
            }

            //delete user

            logger.LogInformation("Deleting user with Id {UserId}", request.UserId);
            var isUserDeleted = await unitOfWork.userRepository.DeleteAsync(user);

            if (!isUserDeleted)
            {
                logger.LogWarning("Failed to Delete user with Id {UserId}", request.UserId);
                return UserErrors.DeleteUserFailed(request.UserId);
            }

            logger.LogInformation("Deleted user with Id {UserId} successfully", request.UserId);

            return true;
        }
    }
}
