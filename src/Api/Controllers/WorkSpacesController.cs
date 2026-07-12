using Application.Common.Dtos;
using Application.Features.WorkSpaces;
using Application.Features.WorkSpaces.commands.CreateWorkSpace;
using Application.Features.WorkSpaces.commands.DeleteWorkSpace;
using Application.Features.WorkSpaces.commands.GetAllUserWorkSpaces;
using Application.Features.WorkSpaces.commands.GetAllWorkSpaces;
using Application.Features.WorkSpaces.commands.GetWorkSpaceById;
using Application.Features.WorkSpaces.commands.UpdateWorkSpace;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/workspaces")]
    [ApiController]
    [Authorize]
    public class WorkSpacesController(IMediator mediator, IAuthorizationService authorizationService) : ControllerBase
    {
        [HttpGet("{id}", Name = "GetWorkSpaceById")]
        public async Task<ActionResult<WorkSpaceDto>> GetWorkSpaceById([FromRoute] long id)
        {
            //check if user is admin
            var isAdmin = User.IsInRole(nameof(Role.Admin));
            if (!isAdmin)
            {
                //check if user is in workSpace
                var authorizationResult = await authorizationService.
                    AuthorizeAsync(User, id, "WorkSpaceUser");

                if (!authorizationResult.Succeeded)
                    return Forbid();
            }


            var result = await mediator.Send(new GetWorkSpaceByIdQuery(id));

            return result.Match(value => Ok(value),
                errors => errors.ToProblemDetailsObjectResult());
        }


        [HttpGet("all", Name = "GetAllWorkSpaces")]
        public async Task<ActionResult<PaginationResultDto<WorkSpaceDto>>> GetAllWorkSpaces([FromQuery] PaginationRequestDto paginationRequestDto)
        {
            //check if user is admin
            var isAdmin = User.IsInRole(nameof(Role.Admin));
            if (isAdmin)
            {
                var getAllWorkSpacesResult = await mediator
                    .Send(new GetAllWorkSpacesQuery(paginationRequestDto));

                return getAllWorkSpacesResult.Match(value => Ok(value), errors =>
                errors.ToProblemDetailsObjectResult());
            }

            //check if user is in workSpace
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var getAllUserWorkSpacesResult = await mediator
                .Send(new GetAllUserWorkSpacesQuery(userId,
                paginationRequestDto));

            return getAllUserWorkSpacesResult.Match(value => Ok(value), errors =>
            errors.ToProblemDetailsObjectResult());
        }


        [HttpPost("", Name = "CreateWorkSpace")]
        public async Task<IActionResult> CreateWorkSpace([FromBody] CreateWorkSpaceDto createWorkSpace)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();


            var result = await mediator.Send(new CreateWorkSpaceCommand(createWorkSpace, userId));

            return result.Match<IActionResult>(value => CreatedAtAction("GetWorkSpaceById", new
            {
                id = value.Id
            }, value),
                errors => errors.ToProblemDetailsObjectResult());
        }


        [HttpPut("{id}", Name = "UpdateWorkSpace")]
        public async Task<IActionResult> UpdateWorkSpace([FromBody] UpdateWorkSpaceDto updateWorkSpaceDto, [FromRoute] long id)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var isAdmin = User.IsInRole(nameof(Role.Admin));
            var authorizationResult = await authorizationService.AuthorizeAsync(User, id,
                "WorkSpaceOwner");
            if (!isAdmin && !authorizationResult.Succeeded)
                return Forbid();



            var result = await mediator.Send(new UpdateWorkSpaceCommand(updateWorkSpaceDto, id, userId));

            return result.Match<IActionResult>(value => NoContent(),
                errors => errors.ToProblemDetailsObjectResult());
        }


        [HttpDelete("{id}", Name = "DeleteWorkSpace")]
        public async Task<IActionResult> DeleteWorkSpace([FromRoute] long id)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var isAdmin = User.IsInRole(nameof(Role.Admin));
            var authorizationResult = await authorizationService.AuthorizeAsync(User, id, "WorkSpaceOwner");
            if (!isAdmin && !authorizationResult.Succeeded)
                return Forbid();



            var result = await mediator.Send(new DeleteWorkSpaceCommand(id, userId));

            return result.Match<IActionResult>(value => NoContent(),
                errors => errors.ToProblemDetailsObjectResult());
        }


    }
}
