using Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.VerfiyOtp
{
    public sealed record VerifyOtpCommand(VerifyOtpDto VerifyOtpDto,OtpPurpose OtpPurpose) : IRequest<ErrorOr<bool>>;
  
}
