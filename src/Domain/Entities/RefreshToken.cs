using Domain.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class RefreshToken: IBaseEntity
    {
        public long Id { get; set; }
        public string Token { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime? RevokedAt { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }

        [NotMapped]
        public bool IsExpired => ExpiresAt < DateTime.UtcNow;

    }
}
