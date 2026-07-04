using Api.Extensions;
using Application.Features.Auth.Commands;
using Application.Features.Auth.Commands.CreateToken;
using Application.Features.Auth.Commands.Login;
using Application.Features.Users.Commands.RegisterNewUser;
using Domain.Common.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IMediator mediator) : ControllerBase
    {
        [HttpPost("register-user", Name = "RegisterUser")]
        public async Task<ActionResult<RegisterUserResultDto>> RegisterUser([FromBody] RegisterUserDto registerUserDto)
        {
            var result = await mediator.Send(new RegisterUserCommand(registerUserDto, Role.User));

            return result.Match(value => Ok(value),
                errors => errors.ToProblemDetailsObjectResult());
        }

        [HttpPost("confirm-email", Name = "ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string email, [FromQuery] string token)
        {
            var result = await mediator.Send(new ConfirmationEmailCommand(email, token));

            return result.Match<IActionResult>(value => NoContent(),
                errors => errors.ToProblemDetailsObjectResult());
        }

        [HttpPost("login", Name = "Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await mediator.Send(new LoginCommand(loginDto));

            if (result.IsError)
            {
                return result.Errors.ToProblemDetailsObjectResult();
            }

            //Add auth info to cookies
            Response.AddAuthInfoToCookies(result.Value.AccessToken, result.Value.RefreshToken);
            return NoContent();
        }

        [HttpPost("refresh-token", Name = "RefreshToken")]
        [Authorize]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.GetValueFromCookie("refresh_token");

            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized();


            var result = await mediator.Send(new CreateTokenCommand(refreshToken));

            if (result.IsError)
            {
                return result.Errors.ToProblemDetailsObjectResult();
            }

            //Add auth info to cookies
            Response.AddAccessTokenToCookies(result.Value);
            return NoContent();
        }

    }
}
