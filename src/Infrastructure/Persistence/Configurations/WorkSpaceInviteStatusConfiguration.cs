using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Configurations
{
    public class WorkSpaceInviteStatusConfiguration : IEntityTypeConfiguration<WorkSpaceInviteStatus>
    {
        public void Configure(EntityTypeBuilder<WorkSpaceInviteStatus> builder)
        {
            builder.ToTable("WorkSpaceInviteStatuses");

            builder.HasKey(w => w.Id);
            builder.Property(w => w.Name).HasMaxLength(100);


            builder.HasData(new WorkSpaceInviteStatus { Id = 1, Name = "Pending" });
            builder.HasData(new WorkSpaceInviteStatus { Id = 2, Name = "Accepted" });
            builder.HasData(new WorkSpaceInviteStatus { Id = 3, Name = "Rejected" });
        }
    }
}
