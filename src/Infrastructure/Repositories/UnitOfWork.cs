using Application.Common.Exceptions;
using Application.Common.Interfaces.Repositories;
using Infrastructure.common.Extensions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext context;
        public IUserRepository userRepository { get; private set; }

        public UnitOfWork(AppDbContext context,IUserRepository userRepository)
        {
            this.context = context;
            this.userRepository = userRepository;
        }


        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await context.Database.BeginTransactionAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException(ex);
            }
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await context.Database.CommitTransactionAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException(ex);
            }
        }

        public async Task DisposeAsync()
        {
            try
            {
                await context.DisposeAsync();
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException(ex);
            }
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await context.Database.RollbackTransactionAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException(ex);
            }
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var rowsAffected = await context.SaveChangesAsync(cancellationToken);
                return rowsAffected;
            }
            catch (DbUpdateException ex) when (ex.IsUniqueConstraintViolation())
            {
                throw new UniqueConstraintViolationException(ex);
            }
            catch (DbUpdateException ex) when (ex.IsForeignKeyConstraintViolation())
            {
                throw new ForeignKeyConstraintViolationException(ex);
            }
            catch (DbUpdateException ex)
            {
                throw new DatabaseOperationException(ex);
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException(ex);
            }
        }
    }
}
