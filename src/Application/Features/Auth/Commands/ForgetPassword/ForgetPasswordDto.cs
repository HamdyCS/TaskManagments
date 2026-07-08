using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.ForgetPassword
{
    public class ForgetPasswordDto
    {
        public string Email { get; set; }

        public string NewPassword { get; set; }

        public string Otp { get; set; }
    }
}
