using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.CreateExternalAuthProperty
{
    public class CreateExternalAuthPropertyCommandHandler(IUnitOfWork unitOfWork
        , ILogger<CreateExternalAuthPropertyCommandHandler> logger)
        : IRequestHandler<CreateExternalAuthPropertyCommand, ErrorOr<AuthenticationProperties>>
    {
        public async Task<ErrorOr<AuthenticationProperties>> Handle(CreateExternalAuthPropertyCommand request, CancellationToken cancellationToken)
        {
            var provider = request.Provider;
            var redirectUrl = request.redirectUrl;
            logger.LogInformation("Starting Create External Auth Property. Provider is {Provider}", provider);

            // generate external auth property
            logger.LogInformation("Generating External Auth Property. Provider is {Provider}", provider);
            var authProperties = unitOfWork.UserRepository.GenerateExternalAuthProperty(provider, redirectUrl);

            if (authProperties is null)
            {
                logger.LogInformation("External Auth Property not found. Provider is {Provider}", provider);
                return LoginByProviderErrors.GenerateExternalAuthPropertyFailed(provider.ToString());
            }

            return authProperties;
        }
    }
}
