using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.SendEmailChangeEmail
{
    public sealed record SendChangeEmailCommand(string NewEmail,string UserId) : IRequest<ErrorOr<bool>>;
   
}
