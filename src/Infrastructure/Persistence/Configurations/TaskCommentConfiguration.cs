using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class TaskCommentConfiguration : IEntityTypeConfiguration<TaskComment>
    {
        public void Configure(EntityTypeBuilder<TaskComment> builder)
        {
            builder.ToTable("TaskComments");

            builder.HasKey(tc => tc.Id);
            builder.Property(tc => tc.Id).ValueGeneratedOnAdd();

            builder.Property(tc => tc.Comment).IsRequired().HasMaxLength(2000);
            builder.Property(tc => tc.CreatedAt).HasDefaultValueSql("GETDATE()");
            builder.Property(tc => tc.LastUpdatedAt).IsRequired(false);

            //composite index
            builder.HasIndex(tc => tc.TaskId);

            builder.HasOne(tc => tc.Task)
                .WithMany(t => t.TaskComments)
                .HasForeignKey(tc => tc.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(tc => tc.CommentBy)
                .WithMany(u => u.TaskComments)
                .HasForeignKey(tc => tc.CommentById)
                .OnDelete(DeleteBehavior.Restrict);

            //query filter
            builder.HasQueryFilter(wu => !wu.Task.IsDeleted);
            builder.HasQueryFilter(wu => !wu.IsDeleted);
        }
    }
}
