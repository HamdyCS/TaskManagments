using Application.Common.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Services
{
    public class OtpService : IOtpService
    {
        public bool VerifyOtp(string otp, string hashOtp)
        {
            return BCrypt.Net.BCrypt.Verify(otp, hashOtp);
        }

        public string GenerateOtp(int length = 6)
        {
            var otp = string.Empty;

            for (int i = 0; i < length; i++)
            {
                otp += new Random().Next(0, 9);
            }

            return otp;
        }

        public string HashOtp(string otp)
        {
            return BCrypt.Net.BCrypt.HashPassword(otp);
        }
        
    }
}
