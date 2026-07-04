using Application.Common.Interfaces.Services;
using Domain.Common.Enums;
using Infrastructure.Common.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Services
{
    public class TokenService(JwtOptions jwtOptions) : ITokenService
    {
        public string GenerateRefreshToken()
        {
            var RandomNumber = new byte[32];

            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(RandomNumber);
            }

            return Convert.ToBase64String(RandomNumber);
        }

        public string GenerateToken(string UserId, string email, Role role)
        {
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new();

            //add claims
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,UserId),
                new Claim(ClaimTypes.Email,email),
                new Claim(ClaimTypes.Role,role.ToString())
            };


            SecurityTokenDescriptor securityTokenDescriptor = new()
            {
                Issuer = jwtOptions.Issuer,
                Audience = jwtOptions.Audience,

                Expires = DateTime.UtcNow.AddMinutes(jwtOptions.LifeTimeMinutes),

                SigningCredentials = new
                (
                    new SymmetricSecurityKey(Convert.FromBase64String(jwtOptions.SigningKey)),
                    SecurityAlgorithms.HmacSha256

                ),

                EncryptingCredentials = new EncryptingCredentials
                (
                    new SymmetricSecurityKey(Convert.FromBase64String(jwtOptions.EncryptionKey)),
                    SecurityAlgorithms.Aes256KW,
                    SecurityAlgorithms.Aes128CbcHmacSha256

                ),


                Subject = new ClaimsIdentity
                (
                   claims
                )
            };

            //create token
            var Token = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);

            //write token as string
            var TokenString = jwtSecurityTokenHandler.WriteToken(Token);

            return TokenString;
        }
    }
}
