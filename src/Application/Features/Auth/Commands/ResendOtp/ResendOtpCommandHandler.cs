using Application.Common.Emails;
using Application.Common.Interfaces.Channels;
using Application.Common.Interfaces.Services;
using Application.Features.Auth.Commands.SendOtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.ResendOtp
{
    public class ResendOtpCommandHandler(ICacheService cacheService, IOtpService otpService,
        IMediator mediator,ILogger<ResendOtpCommandHandler> logger) : IRequestHandler<ResendOtpCommand, ErrorOr<bool>>
    {
        public async Task<ErrorOr<bool>> Handle(ResendOtpCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting resend otp to user with email {Email}", request.ResendOtpDto.Email);

            var otp = otpService.GenerateOtp();
            var hashedOtp = otpService.HashOtp(otp);

            logger.LogInformation("Getting last otp of user with email {Email}", request.ResendOtpDto.Email);

            var lastOtp = await cacheService.GetAsync<OtpDto>($"otp:{request.ResendOtpDto.Email}");

            //check if last otp is not used and not expired and is for the same purpose
            if (lastOtp is not null && (!lastOtp.IsUsed && lastOtp.ExpiresAt < DateTime.UtcNow 
                && lastOtp.OtpPurpose == (byte)request.OtpPurpose))
            {
                logger.LogInformation("Removing last otp of user with email {Email}", request.ResendOtpDto.Email);
                await cacheService.RemoveAsync($"otp:{request.ResendOtpDto.Email}");
            }

            var result = await mediator.Send(new SendOtpCommand(new SendOtpDto { Email = request.ResendOtpDto.Email }, request.OtpPurpose));
            if (result.IsError)
                return result.Errors;


            logger.LogInformation("Resend otp to user with email {Email} Succssfully", request.ResendOtpDto.Email);

            return true;
        }
    }
}
