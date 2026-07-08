using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Errors
{
    public class OtpErrors
    {
        public static Error OtpNotFound(string email) => 
            Error.NotFound("Otp_NotFound", $"Otp not found with email {email}");

        public static Error OtpExpired(string otp) => 
            Error.Unauthorized("Otp_Expired", $"Otp {otp} expired");

        public static Error OtpAlreadyUsed(string otp) => 
            Error.Conflict("Otp_AlreadyUsed", $"Otp {otp} already used");

        public static Error OtpInvalid(string otp) => 
            Error.Unauthorized("Otp_Invalid", $"Otp {otp} invalid");

        public static Error OtpAlreadySent(string email) =>
            Error.Conflict("Otp_AlreadySent", $"Otp already send to user with email {email}");
    }
}
