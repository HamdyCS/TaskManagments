namespace Api.Common.Extensions
{
    public static class HttpRequestExtension
    {
        public static string? GetValueFromCookie(this HttpRequest httpRequest, string key)
        {
            if (httpRequest.Cookies.TryGetValue(key, out var value)) 
                return value;

            return null;
        }
    }
}
