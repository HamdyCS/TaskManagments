using ErrorOr;

namespace Application.Common.Errors
{
    public static class AdminDashboardErrors
    {
        public static Error GetDashboardDataFailed()
            => Error.Failure("AdminDashboard_GetDataFailed", "Failed to get admin dashboard data");

      
    }
}
