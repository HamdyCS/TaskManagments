using Application.Features.TaskAttachments;
using Application.Features.TaskAttachments.Commands.DeleteAttachment;
using Application.Features.TaskAttachments.Commands.UploadAttachment;
using Application.Features.TaskAttachments.Queries.DownloadAttachmentById;
using Application.Features.TaskAttachments.Queries.GetAttachmentById;
using Application.Features.TaskAttachments.Queries.GetAttachmentByName;
using Application.Features.TaskAttachments.Queries.GetAttachmentsByTaskId;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/workspaces/{workspaceId}/projects/{projectId}/tasks/{taskId}/attachments")]
    [ApiController]
    [Authorize]
    public class TaskAttachmentsController(IMediator mediator, IAuthorizationService authorizationService) : ControllerBase
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

        private async Task<bool> _IsWorkSpaceUserAsync(long workspaceId)
        {
            var isWorkSpaceUserResult = await authorizationService.AuthorizeAsync(User, workspaceId, "WorkSpaceUser");
            return isWorkSpaceUserResult.Succeeded;
        }

        [HttpPost(Name = "UploadAttachment")]
        [RequestSizeLimit(52_428_800)]
        [RequestFormLimits(MultipartBodyLengthLimit = 52_428_800)]
        public async Task<IActionResult> UploadAttachment(
            [FromRoute] long workspaceId,
            [FromRoute] long projectId,
            [FromRoute] long taskId,
            [FromForm] UploadAttachmentDto uploadAttachmentDto)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var hasPermission = await _IsAdminOrOwnerOrProductManagerAsync(workspaceId);
            if (!hasPermission) return Forbid();

            var result = await mediator.Send(new UploadAttachmentCommand(uploadAttachmentDto, workspaceId, projectId, taskId, userId));

            return result.Match<IActionResult>(
                value => CreatedAtAction(nameof(GetAttachmentById), new { workspaceId, projectId, taskId, attachmentId = value.Id }, value),
                errors => errors.ToProblemDetailsObjectResult());
        }

        [HttpGet(Name = "GetAttachmentsByTaskId")]
        public async Task<IActionResult> GetAttachmentsByTaskId(
            [FromRoute] long workspaceId,
            [FromRoute] long projectId,
            [FromRoute] long taskId)
        {
            var isWorkSpaceUser = await _IsWorkSpaceUserAsync(workspaceId);
            if (!isWorkSpaceUser) return Forbid();

            var result = await mediator.Send(new GetAttachmentsByTaskIdQuery(workspaceId, projectId, taskId));

            return result.Match<IActionResult>(
                value => Ok(value),
                errors => errors.ToProblemDetailsObjectResult());
        }

        [HttpGet("{attachmentId}", Name = "GetAttachmentById")]
        public async Task<IActionResult> GetAttachmentById(
            [FromRoute] long workspaceId,
            [FromRoute] long projectId,
            [FromRoute] long taskId,
            [FromRoute] long attachmentId)
        {
            var isWorkSpaceUser = await _IsWorkSpaceUserAsync(workspaceId);
            if (!isWorkSpaceUser) return Forbid();

            var result = await mediator.Send(new GetAttachmentByIdQuery(workspaceId, projectId, taskId, attachmentId));

            return result.Match<IActionResult>(
                value => Ok(value),
                errors => errors.ToProblemDetailsObjectResult());
        }


        [HttpGet("by-name/{name}", Name = "GetAttachmentByName")]
        public async Task<IActionResult> GetAttachmentByName(
            [FromRoute] long workspaceId,
            [FromRoute] long projectId,
            [FromRoute] long taskId,
            [FromRoute] string name)
        {
            var isWorkSpaceUser = await _IsWorkSpaceUserAsync(workspaceId);
            if (!isWorkSpaceUser) return Forbid();

            var result = await mediator.Send(new GetAttachmentByNameQuery(workspaceId, projectId, taskId, name));

            return result.Match<IActionResult>(
                value => Ok(value),
                errors => errors.ToProblemDetailsObjectResult());
        }

        [HttpGet("{attachmentId}/download", Name = "DownloadAttachmentById")]
        public async Task<IActionResult> DownloadAttachmentById(
           [FromRoute] long workspaceId,
           [FromRoute] long projectId,
           [FromRoute] long taskId,
           [FromRoute] long attachmentId)
        {
            var isWorkSpaceUser = await _IsWorkSpaceUserAsync(workspaceId);
            if (!isWorkSpaceUser) return Forbid();

            var result = await mediator.Send(new DownloadAttachmentByIdQuery(workspaceId, projectId, taskId, attachmentId));

            return result.Match<IActionResult>(
                value => File(value.FileStream, value.ContentType, value.FileName),
                errors => errors.ToProblemDetailsObjectResult());
        }

        [HttpDelete("{attachmentId}", Name = "DeleteAttachment")]
        public async Task<IActionResult> DeleteAttachment(
            [FromRoute] long workspaceId,
            [FromRoute] long projectId,
            [FromRoute] long taskId,
            [FromRoute] long attachmentId)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var hasPermission = await _IsAdminOrOwnerOrProductManagerAsync(workspaceId);
            if (!hasPermission) return Forbid();

            var result = await mediator.Send(new DeleteAttachmentCommand(workspaceId, projectId, taskId, attachmentId, userId));

            return result.Match<IActionResult>(
                value => NoContent(),
                errors => errors.ToProblemDetailsObjectResult());
        }
    }
}
