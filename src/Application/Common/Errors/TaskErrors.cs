using Application.Features.Tasks;
using Domain.Common.Enums;

namespace Application.Common.Errors
{
    public static class TaskErrors
    {
        public static Error TaskNotFound(long id)
            => Error.NotFound("Task_NotFound", $"Task not found with id {id}");

        public static Error TaskNameAlreadyExists(long projectId, string name)
            => Error.Conflict("Task_NameAlreadyExists", $"Task name '{name}' already exists in project with id {projectId}");

        public static Error ProjectNotFound(long projectId)
            => Error.NotFound("Task_ProjectNotFound", $"Project not found with id {projectId}");

        public static Error UnauthorizedAccess()
            => Error.Forbidden("Task_UnauthorizedAccess", "You are not authorized to perform this action on this task");

        public static Error DeadlineInPast()
            => Error.Validation("Task_DeadlineInPast", "Deadline must be in the future");

        public static Error InvalidStatusTransition(ProjectTaskStatus from, ProjectTaskStatus to)
            => Error.Validation("Task_InvalidStatusTransition", $"Cannot transition from {from} to {to}");

        public static Error DuplicateAssignment(string userId)
            => Error.Conflict("Task_DuplicateAssignment", $"User with id {userId} is already assigned to this task");

        public static Error AssignmentNotFound()
            => Error.NotFound("Task_AssignmentNotFound", "Assignment not found");

        public static Error CreateTaskFailed(long projectId, string userId)
            => Error.Failure("Task_CreateFailed", $"Failed creating task in project with id {projectId} for user with id {userId}");

        public static Error UpdateTaskFailed(long taskId, string userId)
            => Error.Failure("Task_UpdateFailed", $"Failed updating task with id {taskId} for user with id {userId}");

        public static Error DeleteTaskFailed(long taskId, string userId)
            => Error.Failure("Task_DeleteFailed", $"Failed deleting task with id {taskId} for user with id {userId}");

        public static Error ProjectNotInWorkspace(long projectId, long workSpaceId)
            => Error.NotFound("Task_ProjectNotInWorkspace", $"Project with id {projectId} does not belong to workspace with id {workSpaceId}");

        public static Error TaskAssignmentFailed(string assignedUserId, long taskId)
            => Error.Failure("Task_AssignmentFailed", $"Failed assigning task with id {taskId} to user with id {assignedUserId}");

        public static Error RemoveAssignmentFailed(long id, string assignedUserId)
            => Error.Failure("Task_RemoveAssignmentFailed", $"Failed removing assignment with id {id} from assigned user with id {assignedUserId}");

        public static Error NotAssignedToTask(long taskId, string userId)
            => Error.Forbidden("Task_NotAssignedToTask", $"User with id {userId} is not assigned to task with id {taskId}");
    }
}
