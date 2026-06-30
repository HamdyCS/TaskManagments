using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class TaskPriorityConfiguration : IEntityTypeConfiguration<TaskPriority>
    {
        public void Configure(EntityTypeBuilder<TaskPriority> builder)
        {
            builder.ToTable("TaskPriorities");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name).IsRequired().HasMaxLength(50);
            builder.Property(t => t.Description).IsRequired(false).HasMaxLength(250); // Allow Null

            //Low
            //Medium
            //High
            //Critical

            builder.HasData(
                new TaskPriority
                {
                    Id = 1,
                    Name = "Low"
                },
                new TaskPriority
                {
                    Id = 2,
                    Name = "Medium"
                },
                new TaskPriority
                {
                    Id = 3,
                    Name = "High"
                },
                new TaskPriority
                {
                    Id = 4,
                    Name = "Critical"
                });
        }
    }
}