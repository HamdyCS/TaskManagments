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
            Error.Unauthorized("Otp_Expired", $"Otp expired with otp {otp}");

        public static Error OtpAlreadyUsed(string otp) => 
            Error.Conflict("Otp_AlreadyUsed", $"Otp already used with otp {otp}");

        public static Error OtpInvalid(string otp) => 
            Error.Unauthorized("Otp_Invalid", $"Otp invalid with otp {otp}");
    }
}
