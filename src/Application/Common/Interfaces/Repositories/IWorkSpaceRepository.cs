using Domain.Common.Pagination;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces.Repositories
{
    public interface IWorkSpaceRepository : IGenericRepository<WorkSpace>
    {
        Task<PaginationResult<WorkSpace>> GetAllUserWorkSpaces(string userId, int pageNumber, int pageSize);
    }
}
