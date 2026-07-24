using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class ProjectTaskConfiguration : IEntityTypeConfiguration<ProjectTask>
    {
        public void Configure(EntityTypeBuilder<ProjectTask> builder)
        {
            builder.ToTable("Tasks");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedOnAdd();

            builder.Property(t => t.Name).IsRequired().HasMaxLength(200);
            builder.Property(t => t.Description).IsRequired(false).HasMaxLength(2000); // Allow Null

            builder.Property(t => t.CreatedAt).HasDefaultValueSql("GETDATE()");
         
            builder.Property(t => t.LastUpdatedById).IsRequired(false); // Allow Null
            builder.Property(t => t.LastUpdatedAt).IsRequired(false); // Allow Null
            builder.Property(t => t.IsDeleted).HasDefaultValue(false);

            builder.HasOne(t => t.LastUpdatedBy).WithMany()
                .HasForeignKey(t => t.LastUpdatedById);

         

            builder.HasQueryFilter(t => !t.IsDeleted);

            //composite index
            builder.HasIndex(t=>new {t.ProjectId,t.CreatedAt});

            builder.HasOne(t => t.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(t => t.CreatedBy)
                .WithMany(u => u.CreatedTasks)
                .HasForeignKey(t => t.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
