using ErrorOr;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Errors
{
    public class RefreshTokenErrors
    {
        public static Error FailedRevokingRefreshToken(long refreshTokenId) 
            => Error.Failure("RefreshToken_FailedRevoking", $"Failed revoking refresh token with id {refreshTokenId}");

        public static Error RefreshTokenNotFound 
            => Error.NotFound("RefreshToken_NotFound", $"Refresh token not found");

        public static Error CreatedFailed(string userId) =>
            Error.Failure("RefreshToken_CreatedFailed", $"Failed creating refresh token for user with id {userId}");

        public static Error RefreshTokenRevokedOrExpired =>
            Error.Unauthorized("RefreshToken_RevokedOrExpired", "Refresh token revoked or expired");
    }
}
