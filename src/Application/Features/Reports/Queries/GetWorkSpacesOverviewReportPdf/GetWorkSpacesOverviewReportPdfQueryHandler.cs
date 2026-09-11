using Application.Common.Dtos;
using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Application.Features.Reports.Queries.GetWorkSpacesOverviewReport;
using ErrorOr;
using MediatR;

namespace Application.Features.Reports.Queries.GetWorkSpacesOverviewReportPdf
{
    public class GetWorkSpacesOverviewReportPdfQueryHandler(
        IPdfGeneratorService pdfGeneratorService,
       IMediator mediator,
        ILogger<GetWorkSpacesOverviewReportPdfQueryHandler> logger) : IRequestHandler<GetWorkSpacesOverviewReportPdfQuery, ErrorOr<WorkSpacesOverviewReportPdfDto>>
    {
        public async Task<ErrorOr<WorkSpacesOverviewReportPdfDto>> Handle(GetWorkSpacesOverviewReportPdfQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting Get WorkSpaces Overview Report Pdf from {From} to {To} ", request.WorkSpaceOverviewQueryParameters.From, request.WorkSpaceOverviewQueryParameters.To);

            var result = await mediator.Send(new GetWorkSpacesOverviewReportQuery(request.WorkSpaceOverviewQueryParameters), cancellationToken);
            if(result.IsError)
                return  result.Errors;

            logger.LogInformation("Generating PDF for WorkSpaces Overview Report from {From} to {To} ", request.WorkSpaceOverviewQueryParameters.From, request.WorkSpaceOverviewQueryParameters.To);
            var pdfBytes = pdfGeneratorService.GenerateWorkSpacesOverviewReportPdf(result.Value);

            logger.LogInformation("Generated PDF for WorkSpaces Overview Report from {From} to {To} successfully", request.WorkSpaceOverviewQueryParameters.From, request.WorkSpaceOverviewQueryParameters.To);

            return new WorkSpacesOverviewReportPdfDto { PdfBytes = pdfBytes, FileName = $"WorkSpaces-Overview-Report.pdf" };
        }
    }
}
