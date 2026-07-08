using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.ResetPassword
{
    public sealed record ResetPasswordCommand(ResetPasswordDto ResetPasswordDto, string UserId) : IRequest<ErrorOr<bool>>;
   
}
