using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces.Services
{
    public interface IOtpService
    {
        string GenerateOtp(int length = 6);

        string HashOtp(string otp);

        bool VerifyOtp(string otp, string hashOtp);
    }
}
