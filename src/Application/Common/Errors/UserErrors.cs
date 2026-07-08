using Application.Features.Users;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Errors
{
    public static class UserErrors
    {
        public static Error EmailAlreadyExist(string email) => 
            Error.Conflict("User_EmailAlreadyExist", $"Email {email} already exist");

        public static Error UserNotFoundById(string id) => 
            Error.NotFound("User_NotFound", $"User not found with id {id}");

        public static Error UserNotFoundByEmail(string email) => 
            Error.NotFound("User_NotFound", $"User not found with email {email}");

        public static Error RegisterFailed => 
            Error.Failure("User_RegisterFailed", "User register failed");

        public static Error ConfirmEmailFailed(string email) => 
            Error.Failure("User_ConfirmEmailFailed", $"User confirm email {email} failed");

        public static Error UserAlreadyConfirmed(string email) => 
            Error.Conflict("User_AlreadyConfirmed", $"User email {email} already confirmed");

        public static Error UpdateUserFailed(string userId)
            => Error.Failure("User_UpdateFailed", $"Failed update user with id {userId}");

        public static Error UpdatedPasswordFailedByEmail(string email)
            => Error.Failure("User_UpdatedPasswordFailed", $"Failed update password for user with email {email}");

    }
}
