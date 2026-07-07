using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Otps
{
    public class OtpDto
    {
        public string Id { get; set; }

        public string HashOtpCode { get; set; }

        public DateTime CreadtedAt { get; set; }

        public DateTime ExpiresAt { get; set; }

        public byte Type { get; set; }

        public bool IsUsed { get; set; }
    }
}
