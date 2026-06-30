using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class NotificationTypeConfiguration : IEntityTypeConfiguration<NotificationType>
    {
        public void Configure(EntityTypeBuilder<NotificationType> builder)
        {
            builder.ToTable("NotificationTypes");

            builder.HasKey(nt => nt.Id);

            builder.Property(nt => nt.Name).IsRequired().HasMaxLength(100);


            //TaskAssigned = 1
            //CommentAdded  = 2
            //DueDateReminder  = 3

            builder.HasData(
                new NotificationType
                {
                    Id = 1,
                    Name = "TaskAssigned",
                },
                new NotificationType
                {
                    Id = 2,
                    Name = "CommentAdded"
                },
                new NotificationType
                {
                    Id = 3,
                    Name = "DueDateReminder"
                });
        }
    }
}