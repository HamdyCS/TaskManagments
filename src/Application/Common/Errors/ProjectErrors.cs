namespace Application.Common.Errors
{
    public static class ProjectErrors
    {
        public static Error
            ProjectNotFoundById(long id) =>
            Error.NotFound("Project_NotFound", $"Project not found with id {id}");

        public static Error ProjectNameAlreadyExists(long workSpaceId, string name)
            => Error.Conflict("Project_NameAlreadyExists", $"Project name '{name}' already exists in workspace with id {workSpaceId}");

        public static Error CreateProjectFailed(long workSpaceId, string userId)
            => Error.Failure("Project_CreateFailed", $"Failed creating project in workspace with id {workSpaceId} for user with id {userId}");

        public static Error UpdateProjectFailed(long projectId, string userId)
            => Error.Failure("Project_UpdateFailed", $"Failed updating project with id {projectId} for user with id {userId}");

        public static Error DeleteProjectFailed(long projectId, string userId)
            => Error.Failure("Project_DeleteFailed", $"Failed deleting project with id {projectId} for user with id {userId}");

        public static Error WorkSpaceNotFound
            => Error.NotFound("Project_WorkSpaceNotFound", "Workspace not found or deleted");
    }
}
