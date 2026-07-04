using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.Login
{
    public sealed record LoginCommand(LoginDto loginDto): IRequest<ErrorOr<TokenDto>>;
   
}
