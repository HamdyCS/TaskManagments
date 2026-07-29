using Application.Common.Dtos;
using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.Features.Reports.Queries.GetProjectTasksReportByStatus
{
    public class GetProjectTasksReportByStatusQueryHandler(
        IUnitOfWork unitOfWork,
        ICacheService cacheService,
        ILogger<GetProjectTasksReportByStatusQueryHandler> logger) : IRequestHandler<GetProjectTasksReportByStatusQuery, ErrorOr<List<TasksByStatusReportDto>>>
    {
        public async Task<ErrorOr<List<TasksByStatusReportDto>>> Handle(GetProjectTasksReportByStatusQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting GetProjectTasksReportByStatus with projectId {ProjectId}", request.ProjectId);

            var project = await unitOfWork.ProjectRepository.GetByIdAsync(request.ProjectId);
            if (project is null)
                return ProjectErrors.ProjectNotFoundById(request.ProjectId);

            var cacheKey = $"report:tasks-by-status:{request.ProjectId}";

            var cachedResult = await cacheService.GetAsync<List<TasksByStatusReportDto>>(cacheKey);
            if (cachedResult is not null)
            {
                logger.LogInformation("GetProjectTasksReportByStatus with projectId {ProjectId} returned from cache", request.ProjectId);
                return cachedResult;
            }

            var report = await unitOfWork.ReportRepository.GetProjectTasksReportByStatusAsync(request.ProjectId);
            var reportList = report.ToList();

            await cacheService.SetAsync(cacheKey, reportList, TimeSpan.FromMinutes(10));

            logger.LogInformation("GetProjectTasksReportByStatus with projectId {ProjectId} successfully", request.ProjectId);

            return reportList;
        }
    }
}
