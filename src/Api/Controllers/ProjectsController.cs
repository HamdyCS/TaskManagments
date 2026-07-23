using Application.Common.Dtos;
using Application.Features.Projects;
using Application.Features.Projects.Commands.CreateProject;
using Application.Features.Projects.Commands.DeleteProject;
using Application.Features.Projects.Commands.UpdateProject;
using Application.Features.Projects.Commands.UpdateProjectStatus;
using Application.Features.Projects.Queries.GetAllProjects;
using Application.Features.Projects.Queries.GetProjectById;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/workspaces/{workspaceId}/projects")]
    [ApiController]
    [Authorize]
    public class ProjectsController(IMediator mediator, IAuthorizationService authorizationService) : ControllerBase
    {
        private async Task<bool> _IsAdminOrOwnerOrProductManagerAsync(long workspaceId)
        {
            // Check authorization: Admin, Owner, or ProjectManager
            var isAdmin = User.IsInRole(nameof(Role.Admin));
            if (isAdmin)
                return true;

            //Check if use is owner
            var isOwnerAuthResult = await authorizationService.
                AuthorizeAsync(User, workspaceId, "WorkSpaceOwner");
            if (isOwnerAuthResult.Succeeded)
                return true;


            //Check if user is WorkSpaceProjectManager
            var isProductManagerAuthResult = await authorizationService.
                AuthorizeAsync(User, workspaceId, "WorkSpaceProjectManager");
            if (isProductManagerAuthResult.Succeeded)
                return true;

            return false;

        }

        private async Task<bool> _IsAdminOrWorkspaceUserAsync(long workspaceId)
        {
            // Check authorization: Admin, Owner, or ProjectManager
            var isAdmin = User.IsInRole(nameof(Role.Admin));
            if (isAdmin)
                return true;

            //Check if use is owner
            var isOwnerAuthResult = await authorizationService.
                AuthorizeAsync(User, workspaceId, "WorkSpaceUser");
            if (isOwnerAuthResult.Succeeded)
                return true;

            return false;

        }

        [HttpPost("", Name = "CreateProject")]
        public async Task<IActionResult> CreateProject([FromRoute] long workspaceId, [FromBody] CreateProjectDto createProjectDto)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var hasPermission = await _IsAdminOrOwnerOrProductManagerAsync(workspaceId);
            if (!hasPermission)
                return Forbid();


            var result = await mediator.Send(new CreateProjectCommand(createProjectDto, workspaceId, userId));

            return result.Match<IActionResult>(value => CreatedAtAction("GetProjectById", new
            {
                workspaceId,
                projectId = value.Id
            }, value),
                errors => errors.ToProblemDetailsObjectResult());
        }

        [HttpGet("{projectId}", Name = "GetProjectById")]
        public async Task<ActionResult<ProjectDto>> GetProjectById([FromRoute] long workspaceId, [FromRoute] long projectId)
        {
            // Check authorization: Admin or any workspace member
            var hasPermission = await _IsAdminOrWorkspaceUserAsync(workspaceId);
            if (!hasPermission) return Forbid();

            var result = await mediator.Send(new GetProjectByIdQuery(workspaceId, projectId));

            return result.Match(value => Ok(value),
                errors => errors.ToProblemDetailsObjectResult());
        }

        [HttpGet("", Name = "GetAllProjects")]
        public async Task<ActionResult<PaginationResultDto<ProjectDto>>> GetAllProjects([FromRoute] long workspaceId, [FromQuery] PaginationRequestDto paginationRequestDto)
        {
            // Check authorization: Admin or any workspace member
            var hasPermission = await _IsAdminOrWorkspaceUserAsync(workspaceId);
            if (!hasPermission) return Forbid();

            var result = await mediator.Send(
                new GetAllProjectsQuery(workspaceId, paginationRequestDto));

            return result.Match(value => Ok(value),
                errors => errors.ToProblemDetailsObjectResult());
        }

        [HttpPut("{projectId}", Name = "UpdateProject")]
        public async Task<IActionResult> UpdateProject([FromRoute] long workspaceId, [FromRoute] long projectId, [FromBody] UpdateProjectDto updateProjectDto)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            // Check authorization: Admin or any workspace member
            var hasPermission = await _IsAdminOrOwnerOrProductManagerAsync(workspaceId);
            if (!hasPermission) return Forbid();

            var result = await mediator.Send(new UpdateProjectCommand(updateProjectDto, workspaceId, projectId, userId));

            return result.Match<IActionResult>(value => NoContent(),
                errors => errors.ToProblemDetailsObjectResult());
        }

        [HttpPatch("{projectId}/status", Name = "UpdateProjectStatus")]
        public async Task<IActionResult> UpdateProjectStatus(
            [FromRoute] long workspaceId,
            [FromRoute] long projectId,
            [FromBody] UpdateProjectStatusDto updateProjectStatusDto)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var hasPermission = await _IsAdminOrOwnerOrProductManagerAsync(workspaceId);
            if (!hasPermission)
                return Forbid();

            var result = await mediator.Send(new UpdateProjectStatusCommand(updateProjectStatusDto, workspaceId, projectId, userId));

            return result.Match<IActionResult>(value => NoContent(),
                errors => errors.ToProblemDetailsObjectResult());
        }

        [HttpDelete("{projectId}", Name = "DeleteProject")]
        public async Task<IActionResult> DeleteProject([FromRoute] long workspaceId, [FromRoute] long projectId)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            // Check authorization: Admin or any workspace member
            var hasPermission = await _IsAdminOrOwnerOrProductManagerAsync(workspaceId);
            if (!hasPermission) return Forbid();

            var result = await mediator.Send(new DeleteProjectCommand(workspaceId, projectId, userId));

            return result.Match<IActionResult>(value => NoContent(),
                errors => errors.ToProblemDetailsObjectResult());
        }
    }
}
