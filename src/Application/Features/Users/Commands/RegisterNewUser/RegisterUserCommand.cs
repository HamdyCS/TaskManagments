using Domain.Common.Enums;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Users.Commands.RegisterNewUser
{
    public sealed record RegisterUserCommand(RegisterUserDto registerNewUserDto,Roles enRole) : IRequest<ErrorOr<RegisterUserResultDto>>;
    
}
