using Api.Extensions;
using Application.Features.Auth.Commands.CreateToken;
using Application.Features.Auth.Commands.ForgetPassword;
using Application.Features.Auth.Commands.Login;
using Application.Features.Auth.Commands.Logout;
using Application.Features.Auth.Commands.ResendOtp;
using Application.Features.Auth.Commands.SendOtp;
using Application.Features.Users;
using Application.Features.Users.Commands.RegisterNewUser;
using Application.Features.Users.Commands.UpdateUser;
using Application.Features.Users.Queries.GetUserById;
using Domain.Common.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/auth")]
    [Authorize]
    [ApiController]
    public class AuthController(IMediator mediator) : ControllerBase
    {

        [HttpPost("register-user", Name = "RegisterUser")]
        [AllowAnonymous]
        public async Task<ActionResult<RegisterUserResultDto>> RegisterUser([FromBody] RegisterUserDto registerUserDto)
        {
            var result = await mediator.Send(new RegisterUserCommand(registerUserDto, Role.User));

            return result.Match(value => Ok(value),
                errors => errors.ToProblemDetailsObjectResult());
        }


        [HttpPost("confirm-email", Name = "ConfirmEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string email, [FromQuery] string token)
        {
            var result = await mediator.Send(new ConfirmationEmailCommand(email, token));

            return result.Match<IActionResult>(value => NoContent(),
                errors => errors.ToProblemDetailsObjectResult());
        }


        [HttpPost("login", Name = "Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await mediator.Send(new LoginCommand(loginDto));

            if (result.IsError)
            {
                return result.Errors.ToProblemDetailsObjectResult();
            }

            //Add auth info to cookies
            Response.AddAuthInfoToCookie(result.Value.AccessToken, result.Value.RefreshToken);
            return NoContent();
        }


        [HttpPost("refresh-token", Name = "RefreshToken")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken()
        {
            //Get refresh token
            var refreshToken = Request.GetValueFromCookie("refresh_token");

            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized();

            var result = await mediator.Send(new CreateTokenCommand(refreshToken));

            if (result.IsError)
            {
                return result.Errors.ToProblemDetailsObjectResult();
            }

            //Add auth info to cookies
            Response.AddAccessTokenToCookie(result.Value);
            return NoContent();
        }


        [HttpPost("logout", Name = "Logout")]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.GetValueFromCookie("refresh_token");

            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized();


            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await mediator.Send(new LogoutCommand(refreshToken, userId));

            if (result.IsError)
                return result.Errors.ToProblemDetailsObjectResult();

            //Remove auth info from cookies
            Response.RemoveAuthInfoFromCookie();

            return NoContent();
        }

        [HttpGet("", Name = "GetAuthUser")]
        public async Task<ActionResult<UserDto>> GetAuthUser()
        {
            //get user Id
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await mediator.Send(new GetUserByIdQuery(userId));

            return result.Match(value =>
                 Ok(value),
                 errors => errors.ToProblemDetailsObjectResult()
            );
        }


        [HttpPut("", Name = "UpdateAuthUser")]
        public async Task<IActionResult> UpdateAuthUser([FromBody] UpdateUserDto updateUserDto)
        {
            //get user Id
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await mediator.Send(new UpdateUserCommand(userId, updateUserDto));

            return result.Match<IActionResult>(value =>
                CreatedAtRoute("", new
                {

                }, value),
                 errors => errors.ToProblemDetailsObjectResult()
            );
        }

        [HttpPost("forget-password/send-otp", Name = "SendOtpForForgetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> SendOtpForForgetPassword([FromBody] SendOtpDto sendOtpDto)
        {
            var result = await mediator.Send(new SendOtpCommand(sendOtpDto, OtpPurpose.ForgetPassword));

            return result.Match<IActionResult>(value => NoContent(),
                errors => errors.ToProblemDetailsObjectResult());
        }

        [HttpPost("forget-password/resend-otp", Name = "ResendOtpForForgetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResendOtpForForgetPassword([FromBody] ResendOtpDto resendOtpDto)
        {
            var result = await mediator.Send(new ResendOtpCommand(resendOtpDto, OtpPurpose.ForgetPassword));

            return result.Match<IActionResult>(value => NoContent(),
                errors => errors.ToProblemDetailsObjectResult());
        }

        [HttpPost("forget-password", Name = "ForgetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordDto forgetPasswordDto)
        {
            var result = await mediator.Send(new ForgetPasswordCommand(forgetPasswordDto));

            return result.Match<IActionResult>(value => NoContent(),
                errors => errors.ToProblemDetailsObjectResult());
        }
    }
}
