using Application.Common.Emails;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces.Services
{
    public interface IMailService 
    {
        Task SendChnageEmailAsync(ChangeEmailContent changeEmailContent);
        Task SendConfirmationEmailAsync(ConfirmationEmailContent confirmationEmailContent);

        Task SendOtpEmailAsync(OtpEmailContent otpEmailContent);
        Task SendResetPasswordEmailAsync(ResetPasswordEmailContent resetPasswordEmailContent);
    }
}
