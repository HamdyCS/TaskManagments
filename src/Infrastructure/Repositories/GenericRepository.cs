using Application.Common.Interfaces.Repositories;
using Domain.Common.Interfaces;
using Domain.Common.Pagination;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class GenericRepository<T>
        : IGenericRepository<T> where T : class,IBaseEntity
    {
        protected AppDbContext context { get; }

        public GenericRepository(AppDbContext context)
        {
            this.context = context;
        }

        public virtual void Add(T entity)
        {
            context.Set<T>().Add(entity);
        }

        public virtual void AddRange(IEnumerable<T> entities)
        {
            context.Set<T>().AddRange(entities);
        }

        public virtual void Delete(T entity)
        {

            //check if entity is ISoftDelete
            if (entity is ISoftDelete softDelete)
            {
                //update entity
                softDelete.IsDeleted = true;
                softDelete.DeletedAt = DateTime.UtcNow;

                context.Set<T>().Update(entity);

                return;
            }

            //remove entity
            context.Set<T>().Remove(entity);

        }

        public virtual void DeleteRange(IEnumerable<T> entities)
        {

            //هل يمكن تحويل T => ISoftDelete
            if (typeof(ISoftDelete).IsAssignableFrom(typeof(T)))
            {
                foreach (var entity in entities)
                {
                    ISoftDelete softDelete = (ISoftDelete)entity;

                    softDelete.IsDeleted = true;
                    softDelete.DeletedAt = DateTime.UtcNow;

                }

                //update range

                context.Set<T>().UpdateRange(entities);
                return;
            }

            //remove entities
            context.Set<T>().RemoveRange(entities);
        }

        public virtual async Task<PaginationResult<T>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
        {

            var totalCount = await context.Set<T>().CountAsync();
            var data = await context.Set<T>().OrderBy(x=>x.Id).
                Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            //create pagination result
            return new PaginationResult<T>(data, totalCount, pageNumber, pageSize);
        }

        protected virtual async Task<PaginationResult<T>> GetAllByFilterAsync<TKey>(Expression<Func<T, bool>> predicate, int pageNumber = 1, int pageSize = 1,
            Expression<Func<T, TKey>>? orderBy = null,bool ascending = true)
        {

            //query
            var query = context.Set<T>().Where(predicate);

            //ordered query
            var orderedQuery = orderBy == null ?
                ascending ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id) 
                : ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);

            var totalCount = await query.CountAsync();
            var data = await orderedQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            //create pagination result
            return new PaginationResult<T>(data, totalCount, pageNumber, pageSize);
        }

        public virtual async Task<T> GetByIdAsync(long id)
        {
            var entity = await context.Set<T>().FindAsync(id);
            return entity;
        }

        protected virtual async Task<T> GetByFilterAsync(Expression<Func<T, bool>> predicate)
        {

            var entity = await context.Set<T>().FirstOrDefaultAsync(predicate);
            return entity;
        }

        public virtual void Update(T entity)
        {

            context.Set<T>().Update(entity);
        }

        public virtual void UpdateRange(IEnumerable<T> entities)
        {
            context.Set<T>().UpdateRange(entities);
        }

        
    }
}
