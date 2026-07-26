using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class TaskAttachmentConfiguration : IEntityTypeConfiguration<TaskAttachment>
    {
        public void Configure(EntityTypeBuilder<TaskAttachment> builder)
        {
            builder.ToTable("TaskAttachments");

            builder.HasKey(ta => ta.Id);
            builder.Property(ta => ta.Id).ValueGeneratedOnAdd();

            builder.Property(ta => ta.Name).IsRequired().HasMaxLength(255);
            builder.Property(ta => ta.Path).IsRequired().HasMaxLength(2083); // الطول القياسي للـ URLs
            builder.Property(ta => ta.CreatedAt).HasDefaultValueSql("GETDATE()");

            builder.HasOne(ta => ta.CreatedBy)
                .WithMany().HasForeignKey(ta => ta.CreatedById);

            //composite index
            builder.HasIndex(ta => new { ta.TaskId, ta.CreatedAt });

            builder.HasOne(ta => ta.Task)
                .WithMany(t => t.TaskAttachments)
                .HasForeignKey(ta => ta.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            //query filter
            builder.HasQueryFilter(wu => !wu.Task.IsDeleted);
        }
    }
}