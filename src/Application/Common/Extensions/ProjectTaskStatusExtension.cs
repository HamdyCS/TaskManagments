using Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Extensions
{
    public static class ProjectTaskStatusExtension
    {
        public static readonly Dictionary<ProjectTaskStatus, ProjectTaskStatus> validTransitions =
            new Dictionary<ProjectTaskStatus, ProjectTaskStatus>
            {
               { ProjectTaskStatus.Backlog, ProjectTaskStatus.Todo },
               { ProjectTaskStatus.Todo, ProjectTaskStatus.InProgress },
               { ProjectTaskStatus.InProgress, ProjectTaskStatus.Review },
               { ProjectTaskStatus.Review, ProjectTaskStatus.Done },
               { ProjectTaskStatus.Done, ProjectTaskStatus.InProgress }
        };
        public static bool IsValidTransition(this ProjectTaskStatus currentStatus, ProjectTaskStatus newStatus)
        {

            return validTransitions.TryGetValue(currentStatus, out var validNewStatus) &&
                validNewStatus == newStatus;
        }
    }
}
