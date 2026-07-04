using ErrorOr;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Errors
{
    public static class TokenErrors
    {
        public static Error CreatedFailed(string userId) => 
            Error.Failure("Token_CreatedFailed", $"Failed creating token for user with id {userId}");
    }
}
