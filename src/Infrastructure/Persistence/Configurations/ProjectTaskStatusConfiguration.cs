using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class ProjectTaskStatusConfiguration : IEntityTypeConfiguration<ProjectTaskStatus>
    {
        public void Configure(EntityTypeBuilder<ProjectTaskStatus> builder)
        {
            builder.ToTable("ProjectTaskStatuses");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name).IsRequired().HasMaxLength(50);
            builder.Property(t => t.Description).IsRequired(false).HasMaxLength(250); // Allow Null

            //Backlog
            //Todo
            //InProgress
            //Review
            //Done

            builder.HasData(
                new ProjectTaskStatus
                {
                    Id = 1,
                    Name = "Backlog"
                },
                new ProjectTaskStatus
                {
                    Id = 2,
                    Name = "Todo"
                },
                new ProjectTaskStatus
                {
                    Id = 3,
                    Name = "InProgress"
                },
                new ProjectTaskStatus
                {
                    Id = 4,
                    Name = "Review"
                },
                new ProjectTaskStatus
                {
                    Id = 5,
                    Name = "Done"
                });
        }
    }
}