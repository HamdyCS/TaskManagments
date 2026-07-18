using Application.Common.Dtos;
using Application.Features.WorkSpaceInvites;
using Application.Features.WorkSpaceInvites.Commands.CreateWorkSpaceInvite;
using Application.Features.WorkSpaceInvites.Commands.DeleteWorkSpaceInviteByInviteById;
using Application.Features.WorkSpaceInvites.Commands.UpdateWorkSpaceInviteStatus;
using Application.Features.WorkSpaceInvites.Queries.GetAllSendWorkSpaceInvites;
using Application.Features.WorkSpaceInvites.Queries.GetAllUserInvites;
using Application.Features.WorkSpaceInvites.Queries.GetInviteByIdAndInviteToId;
using Application.Features.WorkSpaceInvites.Queries.GetWorkSpaceInviteById;
using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/workspace-invites")]
    [Authorize]
    [ApiController]
    public class WorkSpaceInvitesController(IMediator mediator,IAuthorizationService authorizationService) : ControllerBase
    {
        [HttpGet("{id}", Name = "GetWorkSpaceInviteById")]
        public async Task<ActionResult<WorkSpaceInviteDto>> GetWorkSpaceInviteById([FromRoute] long id)
        {

            ErrorOr<WorkSpaceInviteDto> result;

            //check if user is admin
            var isAdmin = User.IsInRole(nameof(Role.Admin));
            if (isAdmin)
            {
                result = await mediator.Send(new GetInviteByIdQuery(id));

            }
            else
            {
                //not admin
                var userId = User.GetUserId();
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();

                result = await mediator.Send(new GetInviteByIdAndInviteToIdQuery(id, userId));
            }


            return result.Match(value => Ok(value),
                errors => errors.ToProblemDetailsObjectResult());
        }


        [HttpGet("all-my-invites", Name = "GetAllMyWorkSpaceInvites")]
        public async Task<ActionResult<PaginationResultDto<WorkSpaceInviteDto>>> GetAllMyWorkSpaceInvites([FromQuery] PaginationRequestDto paginationRequestDto)
        {

            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await
                mediator.Send(new GetAllUserInvitesQuery(userId, paginationRequestDto));

            return result.Match(value => Ok(value),
                errors => errors.ToProblemDetailsObjectResult());
        }


        [HttpGet("all-my-send-invites", Name = "GetAllMyWorkSpaceSendInvites")]
        public async Task<ActionResult<PaginationResultDto<WorkSpaceInviteDto>>> GetAllWorkMySpaceSendInvites([FromQuery] PaginationRequestDto paginationRequestDto)
        {

            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await
                mediator.Send(new GetAllSendInvitesQuery(userId, paginationRequestDto));

            return result.Match(value => Ok(value),
                errors => errors.ToProblemDetailsObjectResult());
        }

        [HttpPost("", Name = "CreateWorkSpaceInvite")]
        public async Task<IActionResult> CreateWorkSpaceInvite([FromBody] CreateInviteDto createInviteDto)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            //is workspace owner
            var isWorkspaceOwner = await authorizationService
                .AuthorizeAsync(User, createInviteDto.WorkSpaceId,"WorkSpaceOwner");

            if (!isWorkspaceOwner.Succeeded)
                return Forbid();

            var result = await
                mediator.Send(new CreateInviteCommand(createInviteDto, userId));

            return result.Match<IActionResult>(value => CreatedAtAction(nameof(GetWorkSpaceInviteById),new {
                id = value.Id
            }, value),
                errors => errors.ToProblemDetailsObjectResult());
        }

        [HttpDelete("{id}", Name = "DeleteWorkSpaceInvite")]
        public async Task<IActionResult> DeleteWorkSpaceInvite([FromRoute] long id)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await
                mediator.Send(new DeleteInviteByInviteByIdCommand(id, userId));

            return result.Match<IActionResult>(value => NoContent(),
                errors => errors.ToProblemDetailsObjectResult());
        }

        [HttpPatch("{id}/accept", Name = "AcceptWorkSpaceInvite")]
        public async Task<IActionResult> AcceptWorkSpaceInvite([FromRoute] long id)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await mediator.Send(new UpdateInviteStatusCommand(id, userId, WorkSpaceInviteStatus.Accepted));

            return result.Match<IActionResult>(value => NoContent(),
                errors => errors.ToProblemDetailsObjectResult());
        }

        [HttpPatch("{id}/reject", Name = "RejectWorkSpaceInvite")]
        public async Task<IActionResult> RejectWorkSpaceInvite([FromRoute] long id)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await mediator.Send(new UpdateInviteStatusCommand(id, userId, WorkSpaceInviteStatus.Rejected));

            return result.Match<IActionResult>(value => NoContent(),
                errors => errors.ToProblemDetailsObjectResult());
        }
    }
}
