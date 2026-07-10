using Domain.Common.Enums;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.CreateExternalAuthProperty
{
    public sealed record CreateExternalAuthPropertyCommand(Provider Provider,string redirectUrl) : IRequest<ErrorOr<AuthenticationProperties>>;
}
