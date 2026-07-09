using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.VerifyOtp
{
    public class VerifyOtpDto
    {
        public string Otp { get; set; }
        public string Email { get; set; }
    }
}
