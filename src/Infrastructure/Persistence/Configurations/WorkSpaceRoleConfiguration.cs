using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class WorkSpaceRoleConfiguration : IEntityTypeConfiguration<WorkSpaceRoleEntity>
    {
        public void Configure(EntityTypeBuilder<WorkSpaceRoleEntity> builder)
        {
            builder.ToTable("WorkSpaceRoles");

            builder.HasKey(w => w.Id);

            //WorkspaceOwner
            //ProjectManager
            //Member

            builder.HasData(
                new WorkSpaceRoleEntity
                {
                    Id = 1,
                    Name = "WorkspaceOwner"
                },
                new WorkSpaceRoleEntity
                {
                    Id = 2,
                    Name = "ProjectManager"
                },
                new WorkSpaceRoleEntity
                {
                    Id = 3,
                    Name = "Member"
                });
        }
    }
}