using Application.Common.Emails;
using Application.Common.Interfaces.Services;
using Application.Common.Options;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.WebRequestMethods;

namespace Infrastructure.Services
{
    public class MailService(MailOptions mailOptions, ILogger<MailService> logger) : IMailService
    {
       
        public async Task SendConfirmationEmailAsync(ConfirmationEmailContent confirmationEmailContent)
        {
            try
            {
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Common/Templates", "ConfirmationEmail.html");
                string htmlTemplate = await System.IO.File.ReadAllTextAsync(filePath);


                htmlTemplate = htmlTemplate.Replace("{UserName}", confirmationEmailContent.FullName);
                htmlTemplate = htmlTemplate.Replace("{ConfirmationLink}", confirmationEmailContent.Url);


                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Task Managements", mailOptions.Email));
                message.To.Add(new MailboxAddress("", confirmationEmailContent.To));
                message.Subject = confirmationEmailContent.Subject;

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = htmlTemplate,
                    TextBody = confirmationEmailContent.TextBody
                };

                message.Body = bodyBuilder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(mailOptions.Host, mailOptions.Port, MailKit.Security.SecureSocketOptions.StartTls);

                    await client.AuthenticateAsync(mailOptions.Email, mailOptions.AppPassword);

                    await client.SendAsync(message);

                    await client.DisconnectAsync(true);
                }


            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error on Send email {Email}, Error {error}", confirmationEmailContent.To,ex.Message);
            }
        }
    }
}
