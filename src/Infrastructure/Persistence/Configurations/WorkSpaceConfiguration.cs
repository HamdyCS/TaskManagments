using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class WorkSpaceConfiguration : IEntityTypeConfiguration<WorkSpace>
    {
        public void Configure(EntityTypeBuilder<WorkSpace> builder)
        {
            builder.ToTable("WorkSpaces");

            builder.HasKey(w => w.Id);
            builder.Property(w => w.Id).ValueGeneratedOnAdd();

            builder.Property(w => w.Name).IsRequired().HasMaxLength(150);
            builder.Property(w => w.Description).IsRequired(false).HasMaxLength(500); // Allow Null

            builder.Property(w => w.CreatedAt).HasDefaultValueSql("GETDATE()");
            builder.Property(w => w.LastUpdatedById).IsRequired(false);
            builder.Property(w => w.LastUpdatedAt).IsRequired(false); // Allow Null
            builder.Property(w => w.IsDeleted).HasDefaultValue(false);

            // Query Filter لـ Soft Delete
            builder.HasQueryFilter(w => !w.IsDeleted);

            builder.HasOne(w => w.CreatedBy)
                .WithMany(u => u.CreatedWorkSpaces)
                .HasForeignKey(w => w.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(w => w.LastUpdatedBy)
                .WithMany()
                .HasForeignKey(w => w.LastUpdatedById)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
