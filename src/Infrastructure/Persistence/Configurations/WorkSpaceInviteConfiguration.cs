using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Configurations
{
    public class WorkSpaceInviteConfiguration : IEntityTypeConfiguration<WorkSpaceInvite>
    {
        public void Configure(EntityTypeBuilder<WorkSpaceInvite> builder)
        {
            builder.ToTable("WorkSpaceInvites");

            builder.HasKey(w => w.Id);
            builder.HasIndex(w => new { w.WorkSpaceId, w.InvitedById });

            builder.Property(x => x.WorkSpaceId).IsRequired(true);
            builder.Property(x => x.InitedToId).IsRequired(true);
            builder.Property(x => x.InvitedById).IsRequired(true);
            builder.Property(x => x.CreatedAt).IsRequired(true);
            builder.Property(x => x.ExpiresAt).IsRequired(true);

            builder.HasOne(x => x.InvitedBy)
                .WithMany().HasForeignKey(x => x.InvitedById).OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x=>x.InitedTo)
                .WithMany().HasForeignKey(x=>x.InitedToId).OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.WorkSpace)
                .WithMany().HasForeignKey(x => x.WorkSpaceId);


           
            builder.HasQueryFilter(x => !x.WorkSpace.IsDeleted);
        }
    }
}
