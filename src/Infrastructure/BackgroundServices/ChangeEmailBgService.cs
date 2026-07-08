using Application.Common.Interfaces.Channels;
using Application.Common.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.BackgroundServices
{
    public class ChangeEmailBgService(IServiceScopeFactory serviceScopeFactory,
        IChangeEmailQueue changeEmailQueue,
        ILogger<ChangeEmailBgService> logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation("ChangeEmailBgService( service is running waiting for new change email. Time = {TimeNow})", DateTime.UtcNow);
                var changeEmailContent = await changeEmailQueue.DequeueAsync(stoppingToken);

                await using var scope = serviceScopeFactory.CreateAsyncScope();
                var mailService = scope.ServiceProvider.GetRequiredService<IMailService>();

                logger.LogInformation("ChangeEmailBgService( service is running sending change email {Email}. Time = {TimeNow})", changeEmailContent.To, DateTime.UtcNow);
                await mailService.SendChnageEmailAsync(changeEmailContent);
            }
        }
    }
}
