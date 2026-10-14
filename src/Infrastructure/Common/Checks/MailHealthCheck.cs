using Application.Common.Options;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Common.Checks
{
    public class MailHealthCheck(MailOptions mailOptions) : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            using var client = new MailKit.Net.Smtp.SmtpClient();
            {
                try
                {
                    // Connect to the SMTP server and authenticate
                    await client.ConnectAsync(mailOptions.Host, mailOptions.Port, MailKit.Security.SecureSocketOptions.StartTls);

                    await client.AuthenticateAsync(mailOptions.Email, mailOptions.AppPassword);

                    // Disconnect from the SMTP server
                    await client.DisconnectAsync(true, cancellationToken);

                    return HealthCheckResult.Healthy();
                }
                catch (Exception ex)
                {
                    return HealthCheckResult.Unhealthy();
                }
            }
        }
    }
}
