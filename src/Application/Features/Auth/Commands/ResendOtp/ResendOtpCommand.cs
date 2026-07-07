using Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.ResendOtp
{
    public sealed record ResendOtpCommand(ResendOtpDto ResendOtpDto,OtpPurpose OtpPurpose) : IRequest<ErrorOr<bool>>;
  
}
