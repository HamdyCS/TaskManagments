using Application.Common.Interfaces.Channels;
using Application.Common.Interfaces.Services;
using Infrastructure.Common.Channels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.BackgroundServices
{
    public class ResetPasswordEmailBgService(IServiceScopeFactory serviceScopeFactory,
        IResetPasswordEmailQueue resetPasswordEmailQueue,
        ILogger<ResetPasswordEmailBgService> logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation("ResetPasswordEmailBgService( service is running waiting for new reset password email. Time = {TimeNow})", DateTime.UtcNow);
                var resetPasswordEmailContent = await resetPasswordEmailQueue.DequeueAsync(stoppingToken);

                await using var scope = serviceScopeFactory.CreateAsyncScope();
                var mailService = scope.ServiceProvider.GetRequiredService<IMailService>();

                logger.LogInformation("ResetPasswordEmailBgService( service is running sending reset password email {Email}. Time = {TimeNow})", resetPasswordEmailContent.To, DateTime.UtcNow);
                await mailService.SendResetPasswordEmailAsync(resetPasswordEmailContent);
            }
        }
    }
}
