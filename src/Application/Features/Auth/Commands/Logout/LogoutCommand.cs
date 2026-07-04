using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.Logout
{
    public sealed record LogoutCommand(string refreshToken, string userId):IRequest<ErrorOr<bool>>;
   
}
