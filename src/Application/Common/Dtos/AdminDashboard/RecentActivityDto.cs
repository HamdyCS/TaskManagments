using Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos.AdminDashboard
{
    public class RecentActivityDto
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public RecentActivityType ActivityType { get; set; }

        public DateTime CreatedAt { get; set; }

        public IEnumerable<RecentActivityDto> RecentActivityDtos { get; set; }
    }
}
