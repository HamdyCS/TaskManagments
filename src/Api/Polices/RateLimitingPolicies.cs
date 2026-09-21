using System.ComponentModel;

namespace Api.Polices
{
    public static class RateLimitingPolicies
    {
        public static readonly string GlobalRateLimit = "GlobalRateLimit";

        //authentication
        public static readonly string LoginRegisterRateLimit = "LoginRegisterRateLimit";

        public static readonly string AuthenticationActionsRateLimit = "AuthenticationActionsRateLimit";

        public static readonly string RefreshTokenRateLimit = "RefreshTokenRateLimit";

        public static readonly string GlobalAuthenticatedGetRateLimit = "GlobalAuthenticatedGetRateLimit";//for get requests that require authentication

        //for any authenticated request and not get request
        public static readonly string GlobalAuthenticatedWriteRateLimit = "GlobalAuthenticatedWriteRateLimit";



        public static readonly string DashboardRateLimit = "DashboardRateLimit";

        public static readonly string ReportRateLimit = "ReportRateLimit";

        public static readonly string ReportDownloadRateLimit = "ReportDownloadRateLimit";

    }
}
