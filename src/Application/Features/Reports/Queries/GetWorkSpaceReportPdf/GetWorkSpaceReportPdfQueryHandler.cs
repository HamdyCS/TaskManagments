using Application.Common.Dtos;
using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.Features.Reports.Queries.GetWorkSpaceReportPdf
{
    public class GetWorkSpaceReportPdfQueryHandler(
        IPdfGeneratorService pdfGeneratorService,
        IUnitOfWork unitOfWork,
        ICacheService cacheService,
        ILogger<GetWorkSpaceReportPdfQueryHandler> logger) : IRequestHandler<GetWorkSpaceReportPdfQuery, ErrorOr<WorkSpaceReportPdfDto>>
    {
        public async Task<ErrorOr<WorkSpaceReportPdfDto>> Handle(GetWorkSpaceReportPdfQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting GetWorkSpaceReportPdf with workspaceId {WorkspaceId}", request.WorkspaceId);

            var workspace = await unitOfWork.WorkSpaceRepository.GetByIdAsync(request.WorkspaceId);
            if (workspace is null)
                return WorkSpaceErrors.WorkSpaceNotFoundById(request.WorkspaceId);

            var cacheKey = $"report:workspace:{request.WorkspaceId}";

            var cachedResult = await cacheService.GetAsync<WorkSpaceReportDto>(cacheKey);
            if (cachedResult is not null)
            {
                logger.LogInformation("GetWorkSpaceReport with workspaceId {WorkspaceId} returned from cache", request.WorkspaceId);
                var pdfBytesByCache = pdfGeneratorService.GenerateWorkSpaceReportPdf(cachedResult);
                return new WorkSpaceReportPdfDto { PdfBytes = pdfBytesByCache, FileName = $"{workspace.Name}-report.pdf" };
            }

            var report = await unitOfWork.ReportRepository.GetWorkSpaceReportAsync(request.WorkspaceId);

            await cacheService.SetAsync(cacheKey, report, TimeSpan.FromMinutes(10));
            var pdfBytes = pdfGeneratorService.GenerateWorkSpaceReportPdf(report);

            logger.LogInformation("GetWorkSpaceReportPdf with workspaceId {WorkspaceId} successfully", request.WorkspaceId);

            return new WorkSpaceReportPdfDto { PdfBytes = pdfBytes, FileName = $"{workspace.Name}-report.pdf" };
        }
    }
}
