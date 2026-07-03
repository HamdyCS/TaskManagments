using Api.Extensions;
using Application.Features.Users.Commands.RegisterNewUser;
using Domain.Common.Enums;
using MediatR;
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
            var result = await mediator.Send(new RegisterUserCommand(registerUserDto, Roles.User));

            return result.Match(value =>  Ok(value),
                errors =>errors.ToProblemDetailsObjectResult());
        }

        [HttpPost("confirm-email", Name = "ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string email, [FromQuery] string token)
        {
           var result = await mediator.Send(new ConfirmationEmailCommand(email, token));

            return result.Match<IActionResult>(value => NoContent(), 
                errors => errors.ToProblemDetailsObjectResult());
        }
    }
}
