using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.ToTable("Projects");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.Name).IsRequired().HasMaxLength(150);
            builder.Property(p => p.Description).IsRequired(false).HasMaxLength(500); // Allow Null

            builder.Property(p => p.CreatedAt).HasDefaultValueSql("GETDATE()");
            builder.Property(p => p.LastUpdatedAt).IsRequired(false); // Allow Null
            builder.Property(p => p.IsDeleted).HasDefaultValue(false);

            builder.HasQueryFilter(p => !p.IsDeleted);

            //composite index
            builder.HasIndex(p => new { p.WorkSpaceId, p.CreatedAt });

            builder.HasOne(p => p.WorkSpace)
                .WithMany(w => w.Projects)
                .HasForeignKey(p => p.WorkSpaceId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.CreatedBy)
                .WithMany(u => u.CreatedProjects)
                .HasForeignKey(p => p.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
