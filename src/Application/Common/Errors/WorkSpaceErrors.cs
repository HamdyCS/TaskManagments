using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Errors
{
    public static class WorkSpaceErrors
    {
        public static Error 
            WorkSpaceNotFoundById(long id) =>
            Error.NotFound("WorkSpace_NotFound", $"WorkSpace not found with id {id}");

        public static Error CreateWorkSpaceFailed(string userId)
            => Error.Failure("WorkSpace_CreateFailed", $"Failed creating WorkSpace for user with id {userId}");

        public static Error UpdateWorkSpaceFailed(long workSpaceId,string userId)
            => Error.Failure("WorkSpace_UpdateFailed", $"Failed updating WorkSpace with id {workSpaceId} for user with id {userId}");

        public static Error DeleteWorkSpaceFailed(long workSpaceId, string userId)
            => Error.Failure("WorkSpace_DeleteFailed", $"Failed deleting WorkSpace with id {workSpaceId} for user with id {userId}");

        public static Error AddUserToWorkSpaceFailed(string userId, long workSpaceId)
            => Error.Failure("WorkSpace_AddUserFailed", $"Failed adding user with id {userId} to WorkSpace with id {workSpaceId}");
    }
}
