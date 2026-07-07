using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands
{
    public class OtpDto
    {
        public string Email { get; set; }
        public string HashOtp { get; set; }

        public DateTime CreadtedAt { get; set; }

        public DateTime ExpiresAt { get; set; }

        public byte OtpPurpose { get; set; }

        public bool IsUsed { get; set; }

    }
}
