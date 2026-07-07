using Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.SendOtp
{
    public sealed record SendOtpCommand(SendOtpDto SendOtpDto,OtpPurpose OtpPurpose) : IRequest<ErrorOr<bool>>;
  
}
