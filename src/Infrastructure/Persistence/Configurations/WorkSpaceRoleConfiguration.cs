using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class WorkSpaceRoleConfiguration : IEntityTypeConfiguration<WorkSpaceRole>
    {
        public void Configure(EntityTypeBuilder<WorkSpaceRole> builder)
        {
            builder.ToTable("WorkSpaceRoles");

            builder.HasKey(w => w.Id);

            //WorkspaceOwner
            //ProjectManager
            //Member

            builder.HasData(
                new WorkSpaceRole
                {
                    Id = 1,
                    Name = "WorkspaceOwner"
                },
                new WorkSpaceRole
                {
                    Id = 2,
                    Name = "ProjectManager"
                },
                new WorkSpaceRole
                {
                    Id = 3,
                    Name = "Member"
                });
        }
    }
}