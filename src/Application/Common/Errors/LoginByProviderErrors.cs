using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Errors
{
    public static class LoginByProviderErrors
    {
        public static Error GenerateExternalAuthPropertyFailed(string provider) => 
            Error.Failure("LoginByProvider_GenerateExternalAuthPropertyFailed",
                $"Failed generate external auth property for provider {provider}");

       public static Error LoginByProviderFailed => 
            Error.Failure("LoginByProvider_ErrorLoginByProviderFailed",
                $"Error login by provider");
    }
}
