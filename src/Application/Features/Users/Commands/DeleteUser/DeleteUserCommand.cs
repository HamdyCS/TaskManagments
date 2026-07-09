using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.DeleteUser
{
    public sealed record DeleteUserCommand(string UserId,string DeletesBy) : IRequest<ErrorOr<bool>>;
}
