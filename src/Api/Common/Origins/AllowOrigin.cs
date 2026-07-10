using static System.Net.WebRequestMethods;

namespace Api.Common.Origins
{
    public static class AllowOrigin
    {
        private static string[] _origins = new string[]
        {
            "http://localhost:4200"
        };

        public static string[] GetOrigins()
        {
            return _origins;
        }

        public static bool IsAllowed(string url)
        {
            return _origins.Any(allowOrigin=> url.StartsWith(allowOrigin));
        }
    }
}
