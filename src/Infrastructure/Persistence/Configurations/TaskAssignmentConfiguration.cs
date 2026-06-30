using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class TaskAssignmentConfiguration : IEntityTypeConfiguration<TaskAssignment>
    {
        public void Configure(EntityTypeBuilder<TaskAssignment> builder)
        {
            builder.ToTable("TaskAssignments");


            builder.HasKey(ta => ta.Id);
            builder.Property(ta => ta.Id).ValueGeneratedOnAdd();

            builder.Property(ta => ta.CreatedAt).HasDefaultValueSql("GETDATE()");
            builder.Property(ta => ta.UnassignedAt).IsRequired(false);
            builder.Property(ta => ta.IsActive).HasDefaultValue(true);

            //composite index
            builder.HasIndex(ta => new { ta.TaskId, ta.AssignedToId });

            builder.HasOne(ta => ta.Task)
                .WithMany(t => t.TaskAssignments)
                .HasForeignKey(ta => ta.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ta => ta.AssignedBy)
                .WithMany(u => u.GivenAssignments)
                .HasForeignKey(ta => ta.AssignedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ta => ta.AssignedTo)
                .WithMany(u => u.ReceivedAssignments)
                .HasForeignKey(ta => ta.AssignedToId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
