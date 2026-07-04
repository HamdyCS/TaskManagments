using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.CreateRefreshToken
{
    public sealed record CreateRefreshTokenCommand(string userId) : IRequest<ErrorOr<string>>;
   
}
