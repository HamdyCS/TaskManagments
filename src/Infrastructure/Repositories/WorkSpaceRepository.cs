using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class WorkSpaceRepository(AppDbContext context) : GenericRepository<WorkSpace>(context), IWorkSpaceRepository
    {
    }
}
