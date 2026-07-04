using Application.Common.Interfaces.Repositories;
using Application.Features.Auth.Commands.CreateRefreshToken;
using Application.Features.Auth.Commands.CreateToken;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.Login
{
    public class LoginCommandHandler(IUnitOfWork unitOfWork,IMediator mediator,ILogger<LoginCommandHandler> logger) : IRequestHandler<LoginCommand, ErrorOr<TokenDto>>
    {
        public async Task<ErrorOr<TokenDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting login for user with email {Email}", request.loginDto.Email);

            //get user by email and password
            var user = await unitOfWork.userRepository.GetConfirmedByEmailAndPasswordAsync(request.loginDto.Email, request.loginDto.Password);

            if(user is null)
            {
                logger.LogInformation("Invalid credentials for user with email {Email}", request.loginDto.Email);
                return Error.Unauthorized();
            }

            //create refresh token
            var refreshTokenResult = await mediator.Send(new CreateRefreshTokenCommand(user.Id));
            if(refreshTokenResult.IsError)
                return refreshTokenResult.Errors;

            //create token
            var tokenResult = await mediator.Send(new CreateTokenCommand(refreshTokenResult.Value));
            if(tokenResult.IsError)
                return tokenResult.Errors;

            var tokenDto = new TokenDto
            {
                RefreshToken = refreshTokenResult.Value,
                AccessToken = tokenResult.Value
            };

            logger.LogInformation("Login successful for user with email {Email}", request.loginDto.Email);
            return tokenDto;
        }
    }
}
