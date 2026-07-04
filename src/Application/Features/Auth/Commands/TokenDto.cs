using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands
{
    public class TokenDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
