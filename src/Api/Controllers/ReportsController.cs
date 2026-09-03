using Application.Common.Dtos;
using Application.Common.Interfaces.Repositories;
using Application.Features.Reports.Queries.GetProjectTasksReportByPriority;
using Application.Features.Reports.Queries.GetProjectTasksReportByStatus;
using Application.Features.Reports.Queries.GetMemberPerformanceInWorkSpace;
using Application.Features.Reports.Queries.GetMemberPerformanceInProject;
using Application.Features.Reports.Queries.GetWorkSpaceReport;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Application.Features.Reports.Queries.GetWorkSpaceReportPdf;

namespace Api.Controllers
{
    [Route("api/workspaces/{workSpaceId}/reports")]
    [ApiController]
    [Authorize]
    public class ReportsController(IMediator mediator, IAuthorizationService authorizationService, IUnitOfWork unitOfWork) : ControllerBase
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
            var isAdmin = User.IsInRole(nameof(Role.Admin));
            if (isAdmin)
                return true;

            var authResult = await authorizationService.
                AuthorizeAsync(User, workspaceId, "WorkSpaceUser");
            return authResult.Succeeded;
        }

        [HttpGet("projects/{projectId}/tasks-by-priority", Name = "GetProjectTasksReportByPriority")]
        public async Task<IActionResult> GetProjectTasksReportByPriority([FromRoute] long workSpaceId, [FromRoute] long projectId)
        {
         
            var hasPermission = await _IsAdminOrWorkspaceUserAsync(workSpaceId);
            if (!hasPermission)
                return Forbid();

            var result = await mediator.Send(new GetProjectTasksReportByPriorityQuery(projectId));

            return result.Match<IActionResult>(value => Ok(value),
                errors => errors.ToProblemDetailsObjectResult());
        }

        [HttpGet("projects/{projectId}/tasks-by-status", Name = "GetProjectTasksReportByStatus")]
        public async Task<IActionResult> GetProjectTasksReportByStatus([FromRoute] long workSpaceId, [FromRoute] long projectId)
        {

            var hasPermission = await _IsAdminOrWorkspaceUserAsync(workSpaceId);
            if (!hasPermission)
                return Forbid();

            var result = await mediator.Send(new GetProjectTasksReportByStatusQuery(projectId));

            return result.Match<IActionResult>(value => Ok(value),
                errors => errors.ToProblemDetailsObjectResult());
        }
        

        [HttpGet("members/{memberId}/performance", Name = "GetMemberPerformanceInWorkSpace")]
        public async Task<IActionResult> GetMemberPerformanceInWorkSpace([FromRoute] long workspaceId, [FromRoute] string memberId)
        {
            var hasPermission = await _IsAdminOrOwnerOrProductManagerAsync(workspaceId);
            if (!hasPermission)
                return Forbid();

            var result = await mediator.Send(new GetMemberPerformanceInWorkSpaceQuery(workspaceId, memberId));

            return result.Match<IActionResult>(value => Ok(value),
                errors => errors.ToProblemDetailsObjectResult());
        }

        [HttpGet("projects/{projectId}/members/{memberId}/performance", Name = "GetMemberPerformanceInProject")]
        public async Task<IActionResult> GetMemberPerformanceInProject([FromRoute] long workspaceId,[FromRoute] long projectId, [FromRoute] string memberId)
        {

            var hasPermission = await _IsAdminOrWorkspaceUserAsync(workspaceId);
            if (!hasPermission)
                return Forbid();

            var result = await mediator.Send(new GetMemberPerformanceInProjectQuery(projectId, memberId));

            return result.Match<IActionResult>(value => Ok(value),
                errors => errors.ToProblemDetailsObjectResult());
        }

        [HttpGet("", Name = "GetWorkSpaceReport")]
        public async Task<IActionResult> GetWorkSpaceReport([FromRoute] long workspaceId)
        {
            var hasPermission = await _IsAdminOrOwnerOrProductManagerAsync(workspaceId);
            if (!hasPermission)
                return Forbid();

            var result = await mediator.Send(new GetWorkSpaceReportQuery(workspaceId));

            return result.Match<IActionResult>(value => Ok(value),
                errors => errors.ToProblemDetailsObjectResult());
        }

        [HttpGet("pdf/download", Name = "DownloadWorkSpaceReportPdf")]
        public async Task<IActionResult> DownloadWorkSpaceReportPdf([FromRoute] long workspaceId)
        {
            var hasPermission = await _IsAdminOrOwnerOrProductManagerAsync(workspaceId);
            if (!hasPermission)
                return Forbid();

            var result = await mediator.Send(new GetWorkSpaceReportPdfQuery(workspaceId));

            return result.Match<IActionResult>(value => File(value.PdfBytes
                , "application/pdf", value.FileName),
                errors => errors.ToProblemDetailsObjectResult());
        }
    }
}
