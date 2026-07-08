using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.ChangeEmail
{
    public sealed record ChangeEmailCommand(ChangeEmailDto ChangeEmailDto, string UserId) : IRequest<ErrorOr<bool>>;
   
}
