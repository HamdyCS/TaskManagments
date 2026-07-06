using Application.Common.Interfaces.Channels;
using Application.Common.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.BackgroundServices
{
    public class OtpEmailBgService(IOtpEmailQueue otpEmailQueue, IServiceScopeFactory serviceScopeFactory, ILogger<ConfirmationEmailBgService> logger) : BackgroundService
    {
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation("OtpEmailBgService( service is running waiting for new Otp email. Time = {TimeNow})", DateTime.UtcNow);
                var otpEmailContent = await otpEmailQueue.DequeueAsync(stoppingToken);

                await using var scope = serviceScopeFactory.CreateAsyncScope();
                var mailService = scope.ServiceProvider.GetRequiredService<IMailService>();

                logger.LogInformation("OtpEmailBgService( service is running sending Otp email {Email}. Time = {TimeNow})", otpEmailContent.To, DateTime.UtcNow);
                await mailService.SendOtpEmailAsync(otpEmailContent);
            }

        }
    }
}
