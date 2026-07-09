using Application.Common.Emails;
using Application.Common.Errors;
using Application.Common.Interfaces.Channels;
using Application.Common.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.VerifyOtp
{
    public class VerfiyOtpCommandHandler(ICacheService cacheService, IOtpService otpService,
        IConfiguration configuration, IOtpEmailQueue otpEmailQueue,
        ILogger<VerfiyOtpCommandHandler> logger) : IRequestHandler<VerifyOtpCommand, ErrorOr<bool>>
    {
        public async Task<ErrorOr<bool>> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting Verify otp of user with email {Email}", request.VerifyOtpDto.Email);


            logger.LogInformation("Getting otp from cache for user with email {Email}", request.VerifyOtpDto.Email);

            var otpDto = await cacheService.GetAsync<OtpDto>($"otp:{request.OtpPurpose}:{request.VerifyOtpDto.Email}");
            if (otpDto is null)
            {
                logger.LogWarning("Otp not found in cache for user with email {Email}", request.VerifyOtpDto.Email);
                return OtpErrors.OtpNotFound(request.VerifyOtpDto.Email);
            }

            //verify otp code
            if (!otpService.VerifyOtp(request.VerifyOtpDto.Otp, otpDto.HashOtp))
            {
                logger.LogWarning("Invalid otp for user with email {Email}", request.VerifyOtpDto.Email);
                return OtpErrors.OtpInvalid(request.VerifyOtpDto.Otp);
            }

            if (otpDto.OtpPurposeId != (byte)request.OtpPurpose)
            {
                logger.LogWarning("Invalid otp for user with email {Email}", request.VerifyOtpDto.Email);
                return OtpErrors.OtpInvalid(request.VerifyOtpDto.Otp);
            }

            if(otpDto.ExpiresAt < DateTime.UtcNow)
            {
                logger.LogWarning("Otp expired for user with email {Email}", request.VerifyOtpDto.Email);
                return OtpErrors.OtpExpired(request.VerifyOtpDto.Otp);
            }


            //remove otp from cache
            logger.LogInformation("Removing otp from cache for user with email {Email}", request.VerifyOtpDto.Email);
            await cacheService.RemoveAsync($"otp:{request.OtpPurpose}:{request.VerifyOtpDto.Email}",true);
           

            logger.LogInformation("Checking otp for user with email {Email} successfully", request.VerifyOtpDto.Email);
            return true;
        }
    }
}
