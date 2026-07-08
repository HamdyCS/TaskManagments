using Application.Common.Emails;
using Application.Common.Interfaces.Services;
using Application.Common.Options;
using Domain.Common.Enums;
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
        private string _getOtpType(OtpPurpose otpPurpose)
        {
            switch (otpPurpose)
            {
                case OtpPurpose.ForgetPassword:
                    return "Forget Password";
                default:
                    return "Unknown";
            }
        }
       
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
                message.Subject = "Please confirm your email";

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = htmlTemplate,
                    TextBody = "Your Confirmation link is: " + confirmationEmailContent.Url,
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

        public async Task SendResetPasswordEmailAsync(ResetPasswordEmailContent resetPasswordEmailContent)
        {
            try
            {
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Common/Templates", "ResetPasswordEmail.html");
                string htmlTemplate = await System.IO.File.ReadAllTextAsync(filePath);


                htmlTemplate = htmlTemplate.Replace("{UserName}", resetPasswordEmailContent.FullName);
                htmlTemplate = htmlTemplate.Replace("{ResetPasswordLink}", resetPasswordEmailContent.Url);


                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Task Managements", mailOptions.Email));
                message.To.Add(new MailboxAddress("", resetPasswordEmailContent.To));
                message.Subject = "Reset Your Password link";

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = htmlTemplate,
                    TextBody = "Your Reset password link is: " + resetPasswordEmailContent.Url,
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
                logger.LogError(ex, "Error on Send email {Email}, Error {error}", resetPasswordEmailContent.To, ex.Message);
            }
        }

        public async Task SendChnageEmailAsync(ChangeEmailContent changeEmailContent)
        {
            try
            {
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Common/Templates", "ChangeEmail.html");
                string htmlTemplate = await System.IO.File.ReadAllTextAsync(filePath);


                htmlTemplate = htmlTemplate.Replace("{UserName}", changeEmailContent.FullName);
                htmlTemplate = htmlTemplate.Replace("{ChangeEmailLink}", changeEmailContent.Url);


                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Task Managements", mailOptions.Email));
                message.To.Add(new MailboxAddress("", changeEmailContent.To));
                message.Subject = "Change Your Email link";

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = htmlTemplate,
                    TextBody = "Your Change Email link is: " + changeEmailContent.Url,
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
                logger.LogError(ex, "Error on Send email {Email}, Error {error}", changeEmailContent.To, ex.Message);
            }
        }

        public async Task SendOtpEmailAsync(OtpEmailContent otpEmailContent)
        {
            try
            {
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Common/Templates", "OtpEmail.html");
                string htmlTemplate = await System.IO.File.ReadAllTextAsync(filePath);

                //otpType
                htmlTemplate = htmlTemplate.Replace("{OTP_TYPE}", _getOtpType(otpEmailContent.OtpPurpose));
                htmlTemplate = htmlTemplate.Replace("{OTP_Code}", otpEmailContent.OtpCode);
                htmlTemplate = htmlTemplate.Replace("{Valid_Minutes}", otpEmailContent.Valid_Minutes.ToString());


                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Task Managements", mailOptions.Email));
                message.To.Add(new MailboxAddress("", otpEmailContent.To));
                message.Subject = "Task Managements OTP code is: " + otpEmailContent.OtpCode;

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = htmlTemplate,
                    TextBody = "Task Managements OTP code is: " + otpEmailContent.OtpCode,
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
                logger.LogError(ex, "Error on Send email {Email}, Error {error}", otpEmailContent.To, ex.Message);
            }
        }
    }
}
