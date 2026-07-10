using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Application.Features.Auth.Commands.CreateRefreshToken;
using Application.Features.Auth.Commands.CreateToken;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.LoginByProvider
{
    public class LoginByProviderCommandHandler(IUnitOfWork unitOfWork,ITokenService tokenService,IMediator mediator
        , ILogger<LoginByProviderCommandHandler> logger)
        : IRequestHandler<LoginByProviderCommand, ErrorOr<TokenDto>>
    {
        public async Task<ErrorOr<TokenDto>> Handle(LoginByProviderCommand request, CancellationToken cancellationToken)
        {
            var roleOnCreate = request.RoleOnCreate;
            logger.LogInformation("Starting login bu provider");

            // get or create user
            logger.LogInformation("Getting or creating user");
            var user = await unitOfWork.UserRepository.GetOrCreateExternalUserAsync(roleOnCreate);

            if (user is null)
                return LoginByProviderErrors.LoginByProviderFailed;

            //create refresh token
            var refreshTokenResult = await mediator.Send(new CreateRefreshTokenCommand(user.Id));
            if (refreshTokenResult.IsError)
                return refreshTokenResult.Errors;

            //create token
            var tokenResult = await mediator.Send(new CreateTokenCommand(refreshTokenResult.Value));
            if (tokenResult.IsError)
                return tokenResult.Errors;

            var tokenDto = new TokenDto
            {
                RefreshToken = refreshTokenResult.Value,
                AccessToken = tokenResult.Value
            };

            logger.LogInformation("Login by provider successfully");
            return tokenDto;
        }
    }
}
