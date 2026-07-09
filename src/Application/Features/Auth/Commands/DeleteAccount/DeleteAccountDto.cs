using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.DeleteAccount
{
    public record DeleteAccountDto(string Email,string Otp);
   
}
