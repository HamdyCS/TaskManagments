using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notifications");

            builder.HasKey(n => n.Id);
            builder.Property(n => n.Id).ValueGeneratedOnAdd();

            builder.Property(n => n.Title).IsRequired().HasMaxLength(200);
            builder.Property(n => n.Message).IsRequired().HasMaxLength(1000);

            builder.Property(n => n.CreatedAt).HasDefaultValueSql("GETDATE()");
            builder.Property(n => n.IsRead).HasDefaultValue(false);
            builder.Property(n => n.ReadAt).IsRequired(false); // تم تصحيحه من ReadedAt ويقبل Null

            builder.Property(n => n.TaskId).IsRequired(false);
            builder.Property(n => n.WorkSpaceInviteId).IsRequired(false);
           

            //composite index with filter
            builder.HasIndex(n=>new {n.NotifyToId,n.CreatedAt})
                .HasFilter("[IsRead] = 0");
               

            builder.HasOne(n => n.NotifyTo)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.NotifyToId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(n => n.Task)
                .WithMany(t => t.Notifications)
                .HasForeignKey(n => n.TaskId);

            builder.HasOne(n => n.WorkSpaceInvite)
                .WithMany(w => w.Notifications)
                .HasForeignKey(n => n.WorkSpaceInviteId);
               


        }
    }
}
