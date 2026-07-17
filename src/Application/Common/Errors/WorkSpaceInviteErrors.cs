using Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Errors
{
    public static class WorkSpaceInviteErrors
    {
        public static Error WorkSpaceInviteNotFoundById(long id) =>
            Error.NotFound("WorkSpaceInvite_NotFound", $"WorkSpaceInvite not found with id {id}");

        public static Error WorkSpaceInviteNotFoundByIdAndInvitedToId(long id, string InviteToId) =>
            Error.NotFound("WorkSpaceInvite_NotFound", $"WorkSpaceInvite not found with id {id} for invited user with id {InviteToId}");

        public static Error WorkSpaceInviteNotFoundByIdAndInviteById(long id, string InviteById) =>
            Error.NotFound("WorkSpaceInvite_NotFound", $"WorkSpaceInvite not found with id {id} for user with id {InviteById}");

        public static Error UserAlreadyHasPendingInvites(string userId, long workSpaceId) =>
            Error.Conflict("WorkSpaceInvite_UserAlreadyHasPendingInvites", $"User with id {userId} already has pending invite to WorkSpace with id {workSpaceId}");

        public static Error CreateWorkSpaceInviteFailed(string email, long workSpaceId) =>
            Error.Failure("WorkSpaceInvite_CreateFailed", $"Failed creating WorkSpaceInvite for user with email {email} to WorkSpace with id {workSpaceId} to WorkSpace with id {workSpaceId}");

        public static Error DeleteWorkSpaceInviteFailed(long workSpaceInviteId, string inviteById) =>
            Error.Failure("WorkSpaceInvite_DeleteFailed", $"Failed deleting WorkSpaceInvite with id {workSpaceInviteId} for invited user with id {inviteById}");

        public static Error UpdateWorkSpaceStatusInviteFailed(long workSpaceInviteId, string inviteToId,WorkSpaceInviteStatus status) =>
            Error.Failure("WorkSpaceInvite_UpdateFailed", $"Failed updating WorkSpaceInvite status to {status} with id {workSpaceInviteId} for invited user with id {inviteToId} to status {status}");

        public static Error WorkSpaceInviteIsNotPending(long workSpaceInviteId, string inviteById) =>
            Error.Conflict("WorkSpaceInvite_NotPending", $"WorkSpaceInvite with id {workSpaceInviteId} is not pending for inviteBy user with id {inviteById}");

        public static Error WorkSpaceInviteIsNotPendingByInviteTo(long workSpaceInviteId, string inviteToId) =>
            Error.Conflict("WorkSpaceInvite_AlreadyAccepted", $"WorkSpaceInvite with id {workSpaceInviteId} is not pending for invited user with id {inviteToId}");

        public static Error WorkSpaceInviteExpired(long workSpaceInviteId, string inviteToId)
        => Error.Validation("WorkSpaceInvite_Expired", $"WorkSpaceInvite with id {workSpaceInviteId} is expired for user with id {inviteToId}");
    }
}
