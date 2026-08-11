using Application.Common.Emails;
using Application.Common.Errors;
using Application.Common.Interfaces.Channels;
using Application.Common.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.SendOtp
{
    public class SendOtpCommandHandler(ICacheService cacheService, IOtpService otpService,
        IConfiguration configuration, IOtpEmailQueue otpEmailQueue,
        ILogger<SendOtpCommandHandler> logger) : IRequestHandler<SendOtpCommand, ErrorOr<bool>>
    {
        public async Task<ErrorOr<bool>> Handle(SendOtpCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting send otp to user with email {Email}", request.SendOtpDto.Email);


            //getting last otp

            logger.LogInformation("Getting last otp of user with email {Email}", request.SendOtpDto.Email);
            var lastOtp = await cacheService.GetAsync<OtpDto>($"otp:{request.OtpPurpose}:{request.SendOtpDto.Email}");

            //check if last otp is not used and not expired and is for the same purpose
            if (lastOtp is not null && !lastOtp.IsUsed && lastOtp.ExpiresAt > DateTime.UtcNow
                && lastOtp.OtpPurposeId == (byte)request.OtpPurpose)
            {
                logger.LogWarning("Already sent otp");
                //return OtpErrors.OtpAlreadySent(request.SendOtpDto.Email);
                return true;
            }

            //create otp
            var otp = otpService.GenerateOtp();
            var hashedOtp = otpService.HashOtp(otp);

            //get life time in minutes

            logger.LogInformation("getting life time in minutes from config");
            int lifeTimeInMinutes = 1;        
            if(int.TryParse(configuration["Otp:LifeTimeInMinutes"], out var result))
            {
                lifeTimeInMinutes = result;
            }

            logger.LogInformation("Creating otp dto for user with email {Email}", request.SendOtpDto.Email);
            var otpDto = new OtpDto
            {
                CreadtedAt = DateTime.UtcNow,
                OtpPurposeId = (byte)request.OtpPurpose,
                Email = request.SendOtpDto.Email,
                ExpiresAt = DateTime.UtcNow.AddMinutes(lifeTimeInMinutes),
                HashOtp = hashedOtp,
                IsUsed = false,
            };

            //saved to cache
            logger.LogInformation("Saving otp to cache for user with email {Email}", request.SendOtpDto.Email);
            await cacheService.SetAsync($"otp:{request.OtpPurpose}:{request.SendOtpDto.Email}", otpDto, TimeSpan.FromMinutes(lifeTimeInMinutes),true);

            logger.LogInformation("Add otp to queue for user with email {Email}", request.SendOtpDto.Email);
            await otpEmailQueue.EnqueueAsync(new OtpEmailContent
            {
                OtpCode = otp,
                OtpPurpose = request.OtpPurpose,
                To = request.SendOtpDto.Email,
                Valid_Minutes = lifeTimeInMinutes
            });

            logger.LogInformation("Send otp to user with email {Email} Successfully", request.SendOtpDto.Email);
            
            return true;
        }
    }
}
