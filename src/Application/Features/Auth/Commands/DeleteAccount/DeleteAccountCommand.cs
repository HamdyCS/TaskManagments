using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.DeleteAccount
{
    public sealed record DeleteAccountCommand(DeleteAccountDto DeleteAccountDto,string UserId) : IRequest<ErrorOr<bool>>;
}
