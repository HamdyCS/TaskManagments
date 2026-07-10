
using Api.Common.Extensions;
using Application.Common.Dtos;
using Application.Features.Auth.Commands.DeleteAccount;
using Application.Features.Auth.Commands.DeleteUser;
using Application.Features.Users;
using Application.Features.Users.Queries.GetAllUsers;
using Application.Features.Users.Queries.GetUserById;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UsersController(IMediator mediator) : ControllerBase
    {
        [HttpGet("{id}", Name = "GetUserById")]
        public async Task<ActionResult<UserDto>> GetUserById(string id)
        {
            var result = await mediator.Send(new GetUserByIdQuery(id));

            return result.Match(value => Ok(value),
                errors => errors.ToProblemDetailsObjectResult());
        }

        [HttpGet("all", Name = "GetAllUsers")]
        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<ActionResult<PaginationResultDto<UserDto>>> GetAllUsers([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var result = await mediator.Send(new GetAllUsersQuery(
                new PaginationRequestDto { PageNumber = pageNumber, PageSize = pageSize }));

            return result.Match(value => Ok(value),
                errors => errors.ToProblemDetailsObjectResult());
        }

        [HttpDelete("{id}", Name = "DeleteUserById")]
        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<IActionResult> DeleteUserById(string id)
        {
            var deletesBy = User.GetUserId();
            if(string.IsNullOrEmpty(deletesBy)) return Unauthorized();

            var result = await mediator.Send(new DeleteUserCommand(id, deletesBy));

            return result.Match<IActionResult>(value => NoContent(),
                errors => errors.ToProblemDetailsObjectResult());
        }
    }
}
