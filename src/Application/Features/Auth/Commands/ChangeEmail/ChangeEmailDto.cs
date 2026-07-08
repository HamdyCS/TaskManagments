using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.ChangeEmail
{
    public record ChangeEmailDto(string Token, string NewEmail);
    
}
