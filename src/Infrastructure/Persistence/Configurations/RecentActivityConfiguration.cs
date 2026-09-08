using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Configurations
{
    public class RecentActivityConfiguration : IEntityTypeConfiguration<RecentActivity>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<RecentActivity> builder)
        {
            builder.ToTable("RecentActivities");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Text).IsRequired();
            builder.Property(x => x.ActivityType).IsRequired();

            builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        }
    
    }
}
