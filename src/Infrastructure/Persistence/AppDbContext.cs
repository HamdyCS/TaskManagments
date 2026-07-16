using Domain.Entities;
using Infrastructure.Persistence.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectTask> ProjectTasks{ get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens{ get; set; }
        public virtual DbSet<TaskAssignment> TaskAssignments{ get; set; }
        public virtual DbSet<TaskAttachment> TaskAttachments{ get; set; }
        public virtual DbSet<TaskComment> TaskComments{ get; set; }
        public virtual DbSet<WorkSpace> WorkSpaces{ get; set; }
        public virtual DbSet<WorkSpaceUser> WorkSpaceUsers{ get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            //get all as no tracking
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);

        }

    }
}
