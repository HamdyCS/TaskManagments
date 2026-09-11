using Application.Common.Dtos;
using Application.Common.Dtos.WorkSpacesOverview;
using Domain.Common.Pagination;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces.Repositories
{
    public interface IReportRepository
    {
        Task<IEnumerable<TasksByStatusReportDto>> GetProjectTasksReportByStatusAsync(long projectId);
        Task<IEnumerable<TasksByPriorityReportDto>> GetProjectTasksReportByPriorityAsync(long projectId);
        Task<MemberPerformanceDto> GetMemberPerformanceInWorkSpaceAsync(long workspaceId);
        Task<MemberPerformanceDto> GetMemberPerformanceInProjectAsync(long projectId, string memberId);
        Task<WorkSpaceReportDto> GetWorkSpaceReportAsync(long workspaceId);
        Task<PaginationResult<MemberPerformanceDto>> GetAllMemberPerformancesAsync(int pageNumber, int pageSize, string? memberNameQuery);
        Task<WorkSpacesOverviewReportDto?> GetWorkSpacesOverviewAsync(DateTime? from, DateTime? to);
    }
}
