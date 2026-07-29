using Application.Common.Dtos;
using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.Features.Reports.Queries.GetProjectTasksReportByPriority
{
    public class GetProjectTasksReportByPriorityQueryHandler(
        IUnitOfWork unitOfWork,
        ICacheService cacheService,
        ILogger<GetProjectTasksReportByPriorityQueryHandler> logger) : IRequestHandler<GetProjectTasksReportByPriorityQuery, ErrorOr<List<TasksByPriorityReportDto>>>
    {
        public async Task<ErrorOr<List<TasksByPriorityReportDto>>> Handle(GetProjectTasksReportByPriorityQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting GetProjectTasksReportByPriority with projectId {ProjectId}", request.ProjectId);

            var project = await unitOfWork.ProjectRepository.GetByIdAsync(request.ProjectId);
            if (project is null)
                return ProjectErrors.ProjectNotFoundById(request.ProjectId);

            var cacheKey = $"report:tasks-by-priority:{request.ProjectId}";

            var cachedResult = await cacheService.GetAsync<List<TasksByPriorityReportDto>>(cacheKey);
            if (cachedResult is not null)
            {
                logger.LogInformation("GetProjectTasksReportByPriority with projectId {ProjectId} returned from cache", request.ProjectId);
                return cachedResult;
            }

            var report = await unitOfWork.ReportRepository.GetProjectTasksReportByPriorityAsync(request.ProjectId);
            var reportList = report.ToList();

            await cacheService.SetAsync(cacheKey, reportList, TimeSpan.FromMinutes(10));

            logger.LogInformation("GetProjectTasksReportByPriority with projectId {ProjectId} successfully", request.ProjectId);

            return reportList;
        }
    }
}
