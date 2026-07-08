using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.ForgetPassword
{
    public sealed record ForgetPasswordCommand(ForgetPasswordDto ForgetPasswordDto) : IRequest<ErrorOr<bool>>;
}
