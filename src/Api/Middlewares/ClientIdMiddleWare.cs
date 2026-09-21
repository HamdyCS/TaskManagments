namespace Api.Middlewares
{
    public class ClientIdMiddleWare : IMiddleware
    {
        private readonly string _clientIdCookieName = "Client-Id";

  

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if(context.Request.Cookies.TryGetValue(_clientIdCookieName, out var clientId) && !string.IsNullOrWhiteSpace(clientId))
            {
                await next(context);
                return;
            }

            var newClientId = Guid.NewGuid().ToString();

            context.Response.Cookies.Append(_clientIdCookieName, newClientId, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(30)
            });

            await next(context);
        }
    }
}
