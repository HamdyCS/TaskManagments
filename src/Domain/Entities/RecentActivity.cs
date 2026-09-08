using Domain.Common.Enums;
using Domain.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class RecentActivity : IBaseEntity
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public RecentActivityType ActivityType { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
