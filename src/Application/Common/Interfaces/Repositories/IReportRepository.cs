using Application.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces.Repositories
{
    public interface IReportRepository
    {
        Task<IEnumerable<TasksByStatusReportDto>> GetProjectTasksReportByStatusAsync(long projectId);
        Task<IEnumerable<TasksByPriorityReportDto>> GetProjectTasksReportByPriorityAsync(long projectId);
        Task<MemberPerformanceDto> GetMemberPerformanceInWorkSpaceAsync(long workspaceId, string memberId);
        Task<MemberPerformanceDto> GetMemberPerformanceInProjectAsync(long projectId, string memberId);
        Task<WorkSpaceReportDto> GetWorkSpaceReportAsync(long workspaceId);
    }
}
