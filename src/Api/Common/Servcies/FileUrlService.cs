using Application.Common.Interfaces.Services;

namespace Api.Common.Servcies
{
    public class FileUrlService(IHttpContextAccessor httpContextAccessor) : IFileUrlService
    {
        public string GetUrl(string path)
        {
            var request = httpContextAccessor.HttpContext.Request;

            //schema: https
            //host: localhost:7027

            return $"{request.Scheme}://{request.Host}/{path.TrimStart('/')}";
        }
    }
}
