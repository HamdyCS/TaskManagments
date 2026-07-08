using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.ResetPassword
{
    public record ResetPasswordDto(string Token, string NewPassword);
    
}
