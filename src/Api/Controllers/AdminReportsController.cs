using Application.Common.Dtos;
using Application.Common.Dtos.WorkSpaceOverview;
using Application.Common.Dtos.WorkSpacesOverview;
using Application.Features.Reports.Queries.GetAllMemberPerformances;
using Application.Features.Reports.Queries.GetWorkSpacesOverviewReport;
using Application.Features.Reports.Queries.GetWorkSpacesOverviewReportPdf;
using Domain.Common.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/admin/reports")]
    [Authorize(Roles = nameof(Role.Admin))]
    [ApiController]
    public class AdminReportsController(IMediator mediator) : ControllerBase
    {
        [HttpGet("member-performances", Name = "GetAllMemberPerformances")]
        public async Task<ActionResult<PaginationResult<MemberPerformanceDto>>> GetAllMemberPerformances(
            [FromQuery] PaginationRequestDto paginationRequestDto,
            [FromQuery] string? memberName = null)
        {
            var result = await mediator.Send(
                new GetAllMemberPerformancesQuery(paginationRequestDto, memberName));

            return result.Match(value => Ok(value),
                errors => errors.ToProblemDetailsObjectResult());
        }


        [HttpGet("overview", Name = "GetWorkSpacesOverview")]
        public async Task<ActionResult<WorkSpacesOverviewReportDto>> GetWorkSpacesOverview(
                [FromQuery] WorkSpaceOverviewQueryParameters queryParameters)
        {
            var isAdmin = User.IsInRole(nameof(Role.Admin));
            if (!isAdmin)
                return Forbid();

            var result = await mediator.Send(new GetWorkSpacesOverviewReportQuery(queryParameters));

            return result.Match(value => Ok(value),
                errors => errors.ToProblemDetailsObjectResult());
        }

        [HttpGet("overview/pdf/download", Name = "DownloadWorkSpacesOverviewReportPdf")]
        public async Task<IActionResult> DownloadWorkSpacesOverviewReportPdf(
            [FromQuery] WorkSpaceOverviewQueryParameters queryParameters)
        {
         
            var result = await mediator.Send(new 
                GetWorkSpacesOverviewReportPdfQuery(queryParameters));
            return result.Match<IActionResult>(value =>
            {
                return File(value.PdfBytes, "application/pdf", value.FileName);
            },
            errors => errors.ToProblemDetailsObjectResult());
        }
    }
}