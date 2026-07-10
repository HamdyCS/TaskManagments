using Application.Common.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.BackgroundServices
{
    public class RemoveUnConfirmedUsersBgService(IServiceScopeFactory serviceScopeFactory, ILogger<RemoveUnConfirmedUsersBgService> logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("RemoveUnConfirmedUsersBgService( service is running. Time = {TimeNow})", DateTime.UtcNow);

            var timer = new PeriodicTimer(TimeSpan.FromMinutes(30));
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                try
                {
                    await using var scope = serviceScopeFactory.CreateAsyncScope();
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    logger.LogInformation("RemoveUnConfirmedUsersBgService( service is running removing un confirmation users. Time = {TimeNow})", DateTime.UtcNow);
                    await unitOfWork.UserRepository.RemoveUnConfirmedUsersAsync();


                    var rowsAffected = await unitOfWork.SaveChangesAsync();

                    if (rowsAffected > 0)
                    {
                        logger.LogInformation("RemoveUnConfirmedUsersBgService( Removed {RowsAffected} un confirmation users. Time = {TimeNow})", rowsAffected, DateTime.UtcNow);
                    }

                }
                catch
                {
                    logger.LogWarning("RemoveUnConfirmedUsersBgService( Error removing un confirmation users. Time = {TimeNow})", DateTime.UtcNow);
                }

                //delay 30 min
                logger.LogInformation("RemoveUnConfirmedUsersBgService( Waiting 30 min. Time = {TimeNow})", DateTime.UtcNow);

            }
            logger.LogInformation("RemoveUnConfirmedUsersBgService( service is stopping. Time = {TimeNow})", DateTime.UtcNow);

        }
    }
}
