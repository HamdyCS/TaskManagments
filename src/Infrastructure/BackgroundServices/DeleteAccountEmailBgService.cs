using Application.Common.Interfaces.Channels;
using Application.Common.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.BackgroundServices
{
    public class DeleteAccountEmailBgService(IDeleteAccountEmailQueue deleteAccountEmailQueue,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<DeleteAccountEmailBgService> logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation("DeleteAccountEmailBgService( service is running waiting for new delete account email. Time = {TimeNow})", DateTime.UtcNow);
                var deleteAccountEmailContent = await deleteAccountEmailQueue.DequeueAsync(stoppingToken);

                await using var scope = serviceScopeFactory.CreateAsyncScope();
                var mailService = scope.ServiceProvider.GetRequiredService<IMailService>();

                logger.LogInformation("DeleteAccountEmailBgService( service is running sending delete account email {Email}. Time = {TimeNow})", deleteAccountEmailContent.To, DateTime.UtcNow);
                await mailService.SendDeleteAccountEmailAsync(deleteAccountEmailContent);
            }
        }
    }
}
