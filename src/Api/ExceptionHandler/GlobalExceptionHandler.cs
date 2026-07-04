using Application.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Api.ExceptionHandler
{
    public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger,IProblemDetailsService problemDetailsService) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError(exception, "An unhandled exception occurred. TraceId: {TraceId}", Activity.Current?.TraceId);

            ProblemDetails problemDetails = exception switch
            {
                UniqueConstraintViolationException => new ProblemDetails
                {
                    Title = "Unique_Constraint_Violation_Error",
                    Detail = "A unique constraint violation occurred.",
                    Status = StatusCodes.Status409Conflict
                },
                ForeignKeyConstraintViolationException => new ProblemDetails
                {
                    Title = "Foreign_Key_Constraint_Violation_Error",
                    Detail = "A foreign key constraint violation occurred.",
                    Status = StatusCodes.Status409Conflict
                },
                DatabaseOperationException => new ProblemDetails
                {
                    Title = "Database_Operation_Error",
                    Detail = "An unexpected database error occurred.",
                    Status = StatusCodes.Status500InternalServerError
                },
                _ => new ProblemDetails
                {
                    Title = "Unexpected_Server_Error",
                    Detail = "An unexpected server error occurred.",
                    Status = StatusCodes.Status500InternalServerError
                }
            };

            //aad traceId
            problemDetails.Extensions.Add("traceId", Activity.Current?.TraceId.ToString());

            httpContext.Response.StatusCode = problemDetails.Status!.Value;

            await problemDetailsService.WriteAsync(new ProblemDetailsContext
            { 
                HttpContext = httpContext, 
                ProblemDetails = problemDetails 
            });

            return true;
        }
    }
}
