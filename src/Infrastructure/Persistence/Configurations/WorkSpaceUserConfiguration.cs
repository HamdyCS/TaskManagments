using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class WorkSpaceUserConfiguration : IEntityTypeConfiguration<WorkSpaceUser>
    {
        public void Configure(EntityTypeBuilder<WorkSpaceUser> builder)
        {
            builder.ToTable("WorkSpaceUsers");

            //Composite primary key (Has Clustered Index and Unique Constraint)
            builder.HasKey(wu => new { wu.WorkSpaceId, wu.UserId });

            builder.Property(wu => wu.Id).ValueGeneratedOnAdd();

            builder.HasOne(wu => wu.User)
                .WithMany(u => u.WorkSpaceUsers)
                .HasForeignKey(wu => wu.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(wu => wu.WorkSpace)
                .WithMany(w => w.WorkSpaceUsers)
                .HasForeignKey(wu => wu.WorkSpaceId)
                .OnDelete(DeleteBehavior.Cascade);


            //quey filter
            builder.HasQueryFilter(wu => !wu.WorkSpace.IsDeleted);
        }
    }
}