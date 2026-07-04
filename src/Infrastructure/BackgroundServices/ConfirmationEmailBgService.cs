using Application.Common.Interfaces.Channels;
using Application.Common.Interfaces.Services;
using Infrastructure.common.channels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.BackgroundServices
{
    public class ConfirmationEmailBgService(IConfirmationEmailQueue confirmationEmailQueue, IServiceProvider serviceProvider,ILogger<ConfirmationEmailBgService> logger) : BackgroundService
    {
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation("ConfirmationEmailBackgroundService( service is running waiting for new confirmation email. Time = {TimeNow})",DateTime.UtcNow);
                var confirmationEmailContent = await confirmationEmailQueue.DequeueAsync(stoppingToken);
                
               
                using var scope = serviceProvider.CreateScope();
                var mailService = scope.ServiceProvider.GetRequiredService<IMailService>();

                logger.LogInformation("ConfirmationEmailBackgroundService( service is running sending confirmation email {Email}. Time = {TimeNow})", confirmationEmailContent.To, DateTime.UtcNow);
                await mailService.SendConfirmationEmailAsync(confirmationEmailContent);
            }

        }
    }
}
