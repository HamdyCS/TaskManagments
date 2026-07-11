using Domain.Common.Pagination;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Application.Common.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        //get
        Task<PaginationResult<T>> GetAllAsync(int pageNumber, int pageSize);

        Task<T> GetByIdAsync(long id);

        //add
        void Add(T entity);

        void AddRange(IEnumerable<T> entities);

        //update
        void Update(T entity);

        void UpdateRange(IEnumerable<T> entities);

        //delete
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);


    }
}
