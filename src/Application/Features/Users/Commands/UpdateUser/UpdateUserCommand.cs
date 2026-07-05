using Application.Features.Users.Commands.UpdateUser;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Users.Queries.GetUserById
{
    public sealed record UpdateUserCommand(string UserId,UpdateUserDto UpdateUserDto) : IRequest<ErrorOr<UserDto>>;
    
}
