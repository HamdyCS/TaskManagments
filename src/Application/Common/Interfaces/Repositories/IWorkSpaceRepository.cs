using Application.Common.Dtos.WorkSpace;
using Application.Common.Dtos.WorkSpaceOverview;
using Application.Common.Dtos.WorkSpacesOverview;
using Domain.Common.Pagination;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces.Repositories
{
    public interface IWorkSpaceRepository : IGenericRepository<WorkSpace>
    {
        Task<PaginationResult<WorkSpace>> GetAllUserWorkSpaces(string userId, int pageNumber, int pageSize);
        Task<WorkSpaceDetailsDto?> GetWorkSpaceDetailsAsync(long workspaceId);
        Task<string?> GetWorkSpaceNameAsync(long workSpaceId);
        Task<PaginationResult<WorkSpaceOverviewDto>> GetAllWorkSpaceOverviewsAsync(int pageNumber, int pageSize, string? ownerNameQuery, string? workSpaceNameQuery);
    }
}
