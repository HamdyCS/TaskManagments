using Application.Features.TaskComments;
using Application.Features.TaskComments.Commands.CreateComment;
using Application.Features.TaskComments.Commands.DeleteComment;
using Application.Features.TaskComments.Commands.UpdateComment;
using Application.Features.TaskComments.Queries.GetCommentById;
using Application.Features.TaskComments.Queries.GetCommentsByTaskId;
using Application.Common.Dtos;
using Microsoft.AspNetCore.Mvc;
using ErrorOr;
using Application.Features.TaskComments.Commands.DeleteCommentByCommentedById;

namespace Api.Controllers
{
    [Route("api/workspaces/{workspaceId}/projects/{projectId}/tasks/{taskId}/comments")]
    [ApiController]
    [Authorize]
    public class TaskCommentsController(IMediator mediator, IAuthorizationService authorizationService) : ControllerBase
    {
        private async Task<bool> _IsAdminOrOwnerAsync(long workspaceId)
        {
            var isAdmin = User.IsInRole(nameof(Role.Admin));
            if (isAdmin) return true;

            var isOwnerAuthResult = await authorizationService.AuthorizeAsync(User, workspaceId, "WorkSpaceOwner");
            if (isOwnerAuthResult.Succeeded) return true;

            return false;
        }

        private async Task<bool> _IsWorkSpaceUserAsync(long workspaceId)
        {
            var isWorkSpaceUserResult = await authorizationService.AuthorizeAsync(User, workspaceId, "WorkSpaceUser");
            return isWorkSpaceUserResult.Succeeded;
        }

        [HttpPost(Name = "CreateComment")]
        public async Task<IActionResult> CreateComment(
            [FromRoute] long workspaceId,
            [FromRoute] long projectId,
            [FromRoute] long taskId,
            [FromBody] CreateCommentDto createCommentDto)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var isWorkSpaceUser = await _IsWorkSpaceUserAsync(workspaceId);
            if (!isWorkSpaceUser) return Forbid();

            var result = await mediator.Send(new CreateCommentCommand(createCommentDto, workspaceId, projectId, taskId, userId));

            return result.Match<IActionResult>(
                value => CreatedAtAction(nameof(GetCommentById), new { workspaceId, projectId, taskId, commentId = value.Id }, value),
                errors => errors.ToProblemDetailsObjectResult());
        }

        [HttpGet(Name = "GetCommentsByTaskId")]
        public async Task<IActionResult> GetCommentsByTaskId(
            [FromRoute] long workspaceId,
            [FromRoute] long projectId,
            [FromRoute] long taskId,
            [FromQuery] PaginationRequestDto paginationRequestDto)
        {
            var isWorkSpaceUser = await _IsWorkSpaceUserAsync(workspaceId);
            if (!isWorkSpaceUser) return Forbid();

            var result = await mediator.Send(new GetCommentsByTaskIdQuery(workspaceId, projectId, taskId, paginationRequestDto.PageNumber, paginationRequestDto.PageSize));

            return result.Match<IActionResult>(
                value => Ok(value),
                errors => errors.ToProblemDetailsObjectResult());
        }

        [HttpGet("{commentId}", Name = "GetCommentById")]
        public async Task<IActionResult> GetCommentById(
            [FromRoute] long workspaceId,
            [FromRoute] long projectId,
            [FromRoute] long taskId,
            [FromRoute] long commentId)
        {
            var isWorkSpaceUser = await _IsWorkSpaceUserAsync(workspaceId);
            if (!isWorkSpaceUser) return Forbid();

            var result = await mediator.Send(new GetCommentByIdQuery(workspaceId, projectId, taskId, commentId));

            return result.Match<IActionResult>(
                value => Ok(value),
                errors => errors.ToProblemDetailsObjectResult());
        }

        [HttpPut("{commentId}", Name = "UpdateComment")]
        public async Task<IActionResult> UpdateComment(
            [FromRoute] long workspaceId,
            [FromRoute] long projectId,
            [FromRoute] long taskId,
            [FromRoute] long commentId,
            [FromBody] UpdateCommentDto updateCommentDto)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            // Check if the user is the owner of the comment
            var isWorkSpaceUser = await _IsWorkSpaceUserAsync(workspaceId);
            if (!isWorkSpaceUser) return Forbid();

            var result = await mediator.Send(new UpdateCommentCommand(updateCommentDto, workspaceId, projectId, taskId, commentId, userId));

            return result.Match<IActionResult>(
                value => Ok(value),
                errors => errors.ToProblemDetailsObjectResult());
        }

        [HttpDelete("{commentId}", Name = "DeleteComment")]
        public async Task<IActionResult> DeleteComment(
            [FromRoute] long workspaceId,
            [FromRoute] long projectId,
            [FromRoute] long taskId,
            [FromRoute] long commentId)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();


            ErrorOr<Deleted> result;

            // Check if the user is an admin or owner
            var isAdminOrOwner = await _IsAdminOrOwnerAsync(workspaceId);
            if (isAdminOrOwner)
            {
                result = await mediator.Send(new DeleteCommentCommand(workspaceId, projectId, taskId, commentId, userId, isAdminOrOwner));
            }
            else
            {
                //check if the user is the author of the comment
                var isWorkSpaceUser = await _IsWorkSpaceUserAsync(workspaceId);
                if (!isWorkSpaceUser) return Forbid();

                result = await mediator.Send(new DeleteCommentByCommentedByIdCommand(workspaceId, projectId, taskId, commentId, userId, isAdminOrOwner));

            }

            return result.Match<IActionResult>(
                value => NoContent(),
                errors => errors.ToProblemDetailsObjectResult());
        }
    }
}
