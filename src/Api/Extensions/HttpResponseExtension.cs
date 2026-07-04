using Domain.Entities;

namespace Api.Extensions
{
    public static class HttpResponseExtension
    {
        public static HttpResponse AddAuthInfoToCookies(this HttpResponse httpResponse, string accessToken, string refreshToken)
        {
            var cookieOptions = CreateCookieOptions();

            httpResponse.Cookies.Append("access_token", accessToken, cookieOptions);
            httpResponse.Cookies.Append("refresh_token", refreshToken, cookieOptions);

            return httpResponse;
        }

        public static HttpResponse AddAccessTokenToCookies(this HttpResponse httpResponse, string accessToken)
        {
            var cookieOptions = CreateCookieOptions();

            httpResponse.Cookies.Append("access_token", accessToken, cookieOptions);
            return httpResponse;
        }


        public static CookieOptions CreateCookieOptions()
        {
            return new CookieOptions
            {
                Secure = true,
                SameSite = SameSiteMode.None,
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(30),//refresh token expires in 30 days
                Path = "/"
            };
        }
    }
}
