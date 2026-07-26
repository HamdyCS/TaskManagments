using Application.Common.Dtos;
using Application.Features.Tasks;
using Application.Features.Tasks.Commands.AssignUsers;
using Application.Features.Tasks.Commands.ChangeTaskStatus;
using Application.Features.Tasks.Commands.CreateTask;
using Application.Features.Tasks.Commands.DeleteTask;
using Application.Features.Tasks.Commands.RemoveAssignment;
using Application.Features.Tasks.Commands.UpdateTask;
using Application.Features.Tasks.Queries.GetAllProjectTasks;
using Application.Features.Tasks.Queries.GetTaskById;
using Application.Features.Tasks.Queries.GetTasksForUser;
using Application.Features.Tasks.Queries.GetMyTasks;
using Microsoft.AspNetCore.Mvc;
using Application.Features.Tasks.Queries.GetMyTaskById;
using Application.Features.Tasks.Commands.ChangeTaskStatusByAssignedToId;

namespace Api.Controllers
{
    [Route("api/workspaces/{workspaceId}/projects/{projectId}/tasks")]
    [ApiController]
    [Authorize]
    public class ProjectsTasksController(IMediator mediator, IAuthorizationService authorizationService) : ControllerBase
    {
        private async Task<bool> _IsAdminOrOwnerOrProductManagerAsync(long workspaceId)
        {
            var isAdmin = User.IsInRole(nameof(Role.Admin));
            if (isAdmin) return true;

            var isOwnerAuthResult = await authorizationService.AuthorizeAsync(User, workspaceId, "WorkSpaceOwner");
            if (isOwnerAuthResult.Succeeded) return true;

            var isProductManagerAuthResult = await authorizationService.AuthorizeAsync(User, workspaceId, "WorkSpaceProjectManager");
            if (isProductManagerAuthResult.Succeeded) return true;

            return false;
        }

        [HttpPost("", Name = "CreateTask")]
        public async Task<IActionResult> CreateTask([FromRoute] long workspaceId, [FromRoute] long projectId, [FromBody] CreateTaskDto createTaskDto)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            //check if user is admin or owner or product manager
            var hasPermission = await _IsAdminOrOwnerOrProductManagerAsync(workspaceId);
            if (!hasPermission) return Forbid();

            // create task
            var result = await mediator.Send(new CreateTaskCommand(createTaskDto, workspaceId, projectId, userId));

            return result.Match<IActionResult>(
                value => CreatedAtAction("GetTaskById", new { workspaceId, projectId, taskId = value.Id }, value),
                errors => errors.ToProblemDetailsObjectResult());
        }


        [HttpGet("{taskId}", Name = "GetTaskById")]
        public async Task<IActionResult> GetTaskById([FromRoute] long workspaceId, [FromRoute] long projectId, [FromRoute] long taskId)
        {
            var isAdmin = User.IsInRole(nameof(Role.Admin));

            // check if user is admin or workspace user
            var isWorkSpaceUserResult = await authorizationService.
            AuthorizeAsync(User, workspaceId, "WorkSpaceUser");
 
            if (!isAdmin && !isWorkSpaceUserResult.Succeeded) return Forbid();

            // get task by id
            var result = await mediator.Send(new GetTaskByIdQuery(workspaceId, projectId, taskId));

            return result.Match<IActionResult>(
                value => Ok(value),
                errors => errors.ToProblemDetailsObjectResult());
        }


        [HttpGet("{taskId}/me", Name = "GetMyTaskById")]
        public async Task<IActionResult> GetMyTaskById([FromRoute] long workspaceId, [FromRoute] long projectId, [FromRoute] long taskId)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            // check if user is admin or workspace user
            var isWorkSpaceUserResult = await authorizationService.
             AuthorizeAsync(User, workspaceId, "WorkSpaceUser");
            if (!isWorkSpaceUserResult.Succeeded)
                return Forbid();

            // get my task by id
            var result = await mediator.Send(new 
                GetMyTaskByIdQuery(workspaceId, projectId, taskId, userId));

            return result.Match<IActionResult>(
                value => Ok(value),
                errors => errors.ToProblemDetailsObjectResult());
        }

        
        [HttpGet("", Name = "GetAllProjectTasks")]
        public async Task<IActionResult> GetAllProjectTasks([FromRoute] long workspaceId, [FromRoute] long projectId, [FromQuery] PaginationRequestDto paginationRequestDto, [FromQuery] GetAllTasksQueryParameters filterParams)
        {
            // check if user is admin or workspace user
            var isAdmin = User.IsInRole(nameof(Role.Admin));

            var isWorkSpaceUserResult = await authorizationService.
            AuthorizeAsync(User, workspaceId, "WorkSpaceUser");

            if (!isAdmin && !isWorkSpaceUserResult.Succeeded) return Forbid();


            var result = await mediator.Send(
                new GetAllProjectTasksQuery(workspaceId, projectId, paginationRequestDto, filterParams));

            return result.Match<IActionResult>(
                value => Ok(value),
                errors => errors.ToProblemDetailsObjectResult());
        }

        
        [HttpGet("users/{userId}", Name = "GetTasksForUser")]
        public async Task<IActionResult> GetTasksForUser([FromRoute] long workspaceId, [FromRoute] long projectId, [FromRoute] string userId, [FromQuery] PaginationRequestDto paginationRequestDto, [FromQuery] GetAllTasksQueryParameters filterParams)
        {
            // check if user is admin or workspace user
            var isAdmin = User.IsInRole(nameof(Role.Admin));

            var isWorkSpaceUserResult = await authorizationService.
            AuthorizeAsync(User, workspaceId, "WorkSpaceUser");

            if (!isAdmin && !isWorkSpaceUserResult.Succeeded) return Forbid();

            // get tasks for user
            var result = await mediator.Send(
                new GetTasksForUserQuery(workspaceId, projectId, userId, paginationRequestDto, filterParams));

            return result.Match<IActionResult>(
                value => Ok(value),
                errors => errors.ToProblemDetailsObjectResult());
        }

        
        [HttpGet("me", Name = "GetMyTasks")]
        public async Task<IActionResult> GetMyTasks([FromRoute] long workspaceId, [FromRoute] long projectId, [FromQuery] PaginationRequestDto paginationRequestDto, [FromQuery] GetAllTasksQueryParameters filterParams)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            // check if user is workspace user
            var isWorkSpaceUserResult = await authorizationService.
                AuthorizeAsync(User, workspaceId, "WorkSpaceUser");
            if (!isWorkSpaceUserResult.Succeeded)
                return Forbid();

            // get my tasks
            var result = await mediator.Send(
                new GetMyTasksQuery(workspaceId, projectId, userId, paginationRequestDto, filterParams));

            return result.Match<IActionResult>(
                value => Ok(value),
                errors => errors.ToProblemDetailsObjectResult());
        }

        
        [HttpPut("{taskId}", Name = "UpdateTask")]
        public async Task<IActionResult> UpdateTask([FromRoute] long workspaceId, [FromRoute] long projectId, [FromRoute] long taskId, [FromBody] UpdateTaskDto updateTaskDto)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            // check if user is admin or owner or product manager
            var hasPermission = await _IsAdminOrOwnerOrProductManagerAsync(workspaceId);
            if (!hasPermission) return Forbid();

            // update task
            var result = await mediator.Send(
                new UpdateTaskCommand(updateTaskDto, workspaceId, projectId, taskId, userId));

            return result.Match<IActionResult>(
                value => Ok(value),
                errors => errors.ToProblemDetailsObjectResult());
        }

        
        [HttpDelete("{taskId}", Name = "DeleteTask")]
        public async Task<IActionResult> DeleteTask([FromRoute] long workspaceId, [FromRoute] long projectId, [FromRoute] long taskId)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            // check if user is admin or owner or product manager
            var hasPermission = await _IsAdminOrOwnerOrProductManagerAsync(workspaceId);
            if (!hasPermission) return Forbid();

            // delete task
            var result = await mediator.Send(
                new DeleteTaskCommand(workspaceId, projectId, taskId, userId));

            return result.Match<IActionResult>(
                value => NoContent(),
                errors => errors.ToProblemDetailsObjectResult());
        }

        
        [HttpPost("{taskId}/assignments", Name = "AssignUser")]
        public async Task<IActionResult> AssignUser([FromRoute] long workspaceId, [FromRoute] long projectId, [FromRoute] long taskId, [FromBody] AssignUsersDto assignUsersDto)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            // check if user is admin or owner or product manager
            var hasPermission = await _IsAdminOrOwnerOrProductManagerAsync(workspaceId);
            if (!hasPermission) return Forbid();

            // assign user to task
            var result = await mediator.Send(
                new AssignUserCommand(assignUsersDto, workspaceId, projectId, taskId, userId));

            return result.Match<IActionResult>(
                value => Ok(new { assignments = value }),
                errors => errors.ToProblemDetailsObjectResult());
        }

        
        [HttpDelete("{taskId}/assignments/{assignedUserId}", Name = "RemoveAssignment")]
        public async Task<IActionResult> RemoveAssignment([FromRoute] long workspaceId, [FromRoute] long projectId, [FromRoute] long taskId, [FromRoute] string assignedUserId)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            // check if user is admin or owner or product manager
            var hasPermission = await _IsAdminOrOwnerOrProductManagerAsync(workspaceId);
            if (!hasPermission) return Forbid();

            // remove assignment
            var result = await mediator.Send(
                new RemoveAssignmentCommand(workspaceId, projectId, taskId, assignedUserId, userId));

            return result.Match<IActionResult>(
                value => NoContent(),
                errors => errors.ToProblemDetailsObjectResult());
        }

        
        [HttpPatch("{taskId}/status", Name = "ChangeTaskStatus")]
        public async Task<IActionResult> ChangeTaskStatus([FromRoute] long workspaceId, [FromRoute] long projectId, [FromRoute] long taskId, [FromBody] ChangeTaskStatusDto changeTaskStatusDto)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            // check if user is admin or owner or product manager
            var hasPermission = await _IsAdminOrOwnerOrProductManagerAsync(workspaceId);
            if (!hasPermission) return Forbid();


            var result = await mediator.Send(new ChangeTaskStatusCommand(changeTaskStatusDto, workspaceId, projectId, taskId, userId));

            return result.Match<IActionResult>(
                value => Ok(value),
                errors => errors.ToProblemDetailsObjectResult());
        }

        [HttpPatch("{taskId}/me/status", Name = "ChangeMyTaskStatus")]
        public async Task<IActionResult> ChangeMyTaskStatus([FromRoute] long workspaceId, [FromRoute] long projectId, [FromRoute] long taskId, [FromBody] ChangeTaskStatusDto changeTaskStatusDto)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            // check if user is workspace user
            var isWorkSpaceUserResult = await authorizationService.
               AuthorizeAsync(User, workspaceId, "WorkSpaceUser");
            if (!isWorkSpaceUserResult.Succeeded)
                return Forbid();


            var result = await mediator.Send(new ChangeTaskStatusCommandByAssignedToId(changeTaskStatusDto, workspaceId, projectId, taskId, userId));

            return result.Match<IActionResult>(
                value => Ok(value),
                errors => errors.ToProblemDetailsObjectResult());
        }
    }
}
