using Domain.Common.Enums;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.LoginByProvider
{
    public sealed record LoginByProviderCommand(Role RoleOnCreate) : IRequest<ErrorOr<TokenDto>>;
}
