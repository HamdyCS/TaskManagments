using Application.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Api.ExceptionHandler
{
    public sealed class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            ProblemDetails problemDetails = exception switch
            {
                DatabaseOperationException => new ProblemDetails
                {
                    Title = "Database_Operation_Error",
                    Detail = exception.Message,
                    Status = StatusCodes.Status500InternalServerError
                },
                _ => new ProblemDetails
                {
                    Title = "Unexpected_Server_Error",
                    Detail = exception.Message,
                    Status = StatusCodes.Status500InternalServerError
                }
            };

            //aad traceId
            problemDetails.Extensions.Add("traceId", Activity.Current?.TraceId.ToString());

            httpContext.Response.StatusCode = problemDetails.Status!.Value;

            httpContext.Response.ContentType = "application/problem+json";
            await httpContext.Response.WriteAsJsonAsync(new ProblemDetailsContext
            { 
                HttpContext = httpContext, 
                ProblemDetails = problemDetails 
            }, cancellationToken);

            return true;
        }
    }
}
