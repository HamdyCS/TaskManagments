using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.CreateToken
{
    public sealed record CreateTokenCommand(string refreshToken,string userId) : IRequest<ErrorOr<string>>;
    
}
