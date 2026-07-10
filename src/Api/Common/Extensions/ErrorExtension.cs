using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Api.Common.Extensions
{
    public static class ErrorExtension
    {
        public static ObjectResult ToProblemDetailsObjectResult(this List<Error> errors)
        {
            //check if there are any errors
            if (!errors.Any())
            {
                var unKnownProblemDetails = new ProblemDetails
                {
                    Title = "Unknown_Error",
                    Status = StatusCodes.Status500InternalServerError
                };

                return new ObjectResult(unKnownProblemDetails)
                {
                    StatusCode = unKnownProblemDetails.Status
                };
            }

            //get the first error
            var firstError = errors.First();

            //create the problem details
            ProblemDetails problemDetails = firstError.Type switch
            {
                ErrorType.Validation => new ValidationProblemDetails(
                    errors.GroupBy(e => e.Code).ToDictionary(g => g.Key,
                    g => g.Select(e => e.Description).ToArray())
                    )
                {
                    Title = "Validation_Error",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = firstError.Description
                },
                ErrorType.NotFound => new ProblemDetails
                {
                    Title = firstError.Code,
                    Status = StatusCodes.Status404NotFound,
                    Detail = firstError.Description
                },
                ErrorType.Conflict => new ProblemDetails
                {
                    Title = firstError.Code,
                    Status = StatusCodes.Status409Conflict,
                    Detail = firstError.Description
                },
                ErrorType.Unauthorized => new ProblemDetails
                {
                    Title = firstError.Code,
                    Status = StatusCodes.Status401Unauthorized,
                    Detail = firstError.Description
                },
                ErrorType.Forbidden => new ProblemDetails
                {
                    Title = firstError.Code,
                    Status = StatusCodes.Status403Forbidden,
                    Detail = firstError.Description
                },
                ErrorType.Failure => new ProblemDetails
                {
                    Title = firstError.Code,
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = firstError.Description
                },
                _ => new ProblemDetails
                {
                    Title = firstError.Code,
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = firstError.Description
                }
            };

            //add trace id
            problemDetails.Extensions.Add("traceId",Activity.Current?.TraceId.ToString());

            return new ObjectResult(problemDetails)
            {
                StatusCode = problemDetails.Status
            };
        }

    }
}

