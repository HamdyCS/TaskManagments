using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Common.Options
{
    public class JwtOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int LifeTimeMinutes { get; set; }
        public string SigningKey { get; set; }
        public string EncryptionKey { get; set; }
    }
}
