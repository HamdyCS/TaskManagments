using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.VerfiyOtp
{
    public class VerifyOtpDto
    {
        public string Otp { get; set; }
        public string Email { get; set; }
    }
}
