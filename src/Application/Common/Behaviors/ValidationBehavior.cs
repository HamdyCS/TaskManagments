using ErrorOr;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse>
        (IEnumerable<IValidator<TRequest>> validators)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            //check if request has any validators
            if (!validators.Any())
            {
                return await next();
            }

            //create validation context
            var context = new ValidationContext<TRequest>(request);

            //get all failures
            var failures = await Task.WhenAll(
                validators.Select(v=>v.ValidateAsync(context, cancellationToken)));

            //get all errors
            var errors = failures.SelectMany(f => f.Errors)
                .Where(e => e is not null)
                .Select(e=> Error.Validation(e.PropertyName,e.ErrorMessage)).ToList();

            //check if there are any errors
            if(errors.Any())
            {
                return (dynamic)errors;
            }
            
            return await next();
        }
    }
}
