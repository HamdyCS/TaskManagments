using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.SendPasswordResetEmail
{
    public sealed record SendPasswordResetEmailCommand(string UserId) : IRequest<ErrorOr<bool>>;
   
}
