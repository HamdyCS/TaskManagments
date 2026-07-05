using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Users.Queries.GetUserById
{
    public sealed record GetUserByIdQuery(string userId) : IRequest<ErrorOr<UserDto>>;
    
}
