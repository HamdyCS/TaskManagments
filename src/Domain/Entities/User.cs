using Domain.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class User : IdentityUser, ISoftDelete
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public short RoleId { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public DateTime CreatedAt { get; set; }

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";


        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
        public virtual ICollection<WorkSpaceUser> WorkSpaceUsers { get; set; }
        public virtual ICollection<WorkSpace> CreatedWorkSpaces { get; set; }
        public virtual ICollection<Project> CreatedProjects { get; set; }
        public virtual ICollection<ProjectTask> CreatedTasks { get; set; }
        public virtual ICollection<TaskAssignment> GivenAssignments { get; set; }
        public virtual ICollection<TaskAssignment> ReceivedAssignments { get; set; }
        public virtual ICollection<TaskComment> TaskComments { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
