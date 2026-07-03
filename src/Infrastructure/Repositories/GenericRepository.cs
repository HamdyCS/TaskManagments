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
    public class GenericRepository<T>(ILogger<GenericRepository<T>> logger, AppDbContext context)
        : IGenericRepository<T> where T : class
    {
        private string _entityName => typeof(T).Name;

        public void Add(T entity)
        {
            logger.LogInformation("Adding a new {EntityType}", _entityName);
            context.Set<T>().Add(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            logger.LogInformation("Adding a new range of {EntityType}", _entityName);
            context.Set<T>().AddRange(entities);
        }

        public void Delete(T entity)
        {
            logger.LogInformation("Deleting a {EntityType}", _entityName);

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

        public void DeleteRange(IEnumerable<T> entities)
        {
            logger.LogInformation("Deleting range of a {EntityType}", _entityName);

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

        public async Task<PaginationResult<T>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
        {
            logger.LogInformation("Getting all of {EntityType} page {PageNumber}, size {PageSize}", _entityName, pageNumber, pageSize);
            
            var totalCount = await context.Set<T>().CountAsync();
            var data = await context.Set<T>().Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            //create pagination result
            return new PaginationResult<T>(data, totalCount, pageNumber, pageSize);
        }

        protected async Task<PaginationResult<T>> GetAllByFilterAsync(Expression<Func<T, bool>> predicate, int pageNumber = 1, int pageSize = 1)
        {
            logger.LogInformation("Getting all with filter of {EntityType} page {PageNumber}, size {PageSize}", _entityName, pageNumber, pageSize);

            //query
            var query = context.Set<T>().Where(predicate);

            var totalCount = await query.CountAsync();
            var data = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize)
                .ToListAsync();

            //create pagination result
            return new PaginationResult<T>(data, totalCount, pageNumber, pageSize);
        }

        public T GetByIdAsync(long id)
        {
            logger.LogInformation("Getting {EntityType} by id {Id}", _entityName, id);

            var entity = context.Set<T>().Find(id);
            return entity;
        }

        public void Update(T entity)
        {
            logger.LogInformation("Updating a {EntityType}", _entityName);

            context.Set<T>().Update(entity);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            logger.LogInformation("Updating range of a {EntityType}", _entityName);

            context.Set<T>().UpdateRange(entities);
        }

    }
}
