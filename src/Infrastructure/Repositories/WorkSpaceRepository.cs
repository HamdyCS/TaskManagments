using Application.Common.Dtos;
using Application.Common.Dtos.WorkSpace;
using Application.Common.Interfaces.Repositories;
using Domain.Common.Enums;
using Domain.Common.Pagination;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class WorkSpaceRepository(AppDbContext context) : GenericRepository<WorkSpace>(context), IWorkSpaceRepository
    {
        public async Task<PaginationResult<WorkSpace>> GetAllUserWorkSpaces(string userId, int pageNumber, int pageSize)
        {
            var query = context.WorkSpaces
                .Include(ws => ws.CreatedBy)
                .Where(ws => ws.WorkSpaceUsers.
            Any(wu => wu.UserId == userId));

            var totalCount = await query.CountAsync();
            var workSpaces = await query.OrderBy(ws => ws.CreatedAt)
                .Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginationResult<WorkSpace>(workSpaces, totalCount, pageNumber, pageSize);
        }

        public async Task<string?> GetWorkSpaceNameAsync(long workSpaceId)
            => await context.WorkSpaces.Include(ws => ws.CreatedBy).Where(ws => ws.Id == workSpaceId)
               .Select(ws => ws.Name).FirstOrDefaultAsync();

        public async Task<PaginationResult<WorkSpaceOverviewDto>> GetAllWorkSpaceOverviewsAsync(int pageNumber, int pageSize, string? ownerNameQuery, string? workSpaceNameQuery)
        {
            var query = context.WorkSpaces.Where(ws => true);

            if(!string.IsNullOrEmpty(ownerNameQuery))
            {
                query = query.Where(ws => ws.WorkSpaceUsers.Any(wu => wu.WorkSpaceRole == 
                WorkSpaceRole.Owner &&
                (wu.User.FirstName.Contains(ownerNameQuery) || wu.User.LastName.Contains(ownerNameQuery))));
            }

            if (!string.IsNullOrEmpty(workSpaceNameQuery))
            {
                query = query.Where(ws => ws.Name.Contains(workSpaceNameQuery));
            }
            

            var totalCount = query.Count();
            
            var workSpaceOverviews = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(ws => new WorkSpaceOverviewDto
            {
                Id = ws.Id,
                OwnersNames = ws.WorkSpaceUsers.Where(wu => wu.WorkSpaceRole == WorkSpaceRole.Owner)
                .Select(wu => wu.User.FullName),
                MembersCount = ws.WorkSpaceUsers.Count(),
                ProjectsCount = ws.Projects.Count(),
                TasksCount = ws.Projects.SelectMany(p => p.Tasks).Count(),
                CreatedAt = ws.CreatedAt,
                Name = ws.Name,
            }).ToListAsync();


         

            return new PaginationResult<WorkSpaceOverviewDto>(workSpaceOverviews, totalCount, pageNumber, pageSize);
        }

        public async Task<WorkSpaceDetailsDto?> GetWorkSpaceDetailsAsync(long workspaceId)
        {
            var result = await context.WorkSpaces.Where(ws => ws.Id == workspaceId).Select(ws => new WorkSpaceDetailsDto
            {
                WorkSpaceOverview = new WorkSpaceOverviewDto
                {
                    Id = ws.Id,
                    OwnersNames = ws.WorkSpaceUsers.Where(wu => wu.WorkSpaceRole == WorkSpaceRole.Owner)
                .Select(wu => wu.User.FullName),
                    MembersCount = ws.WorkSpaceUsers.Count(),
                    ProjectsCount = ws.Projects.Count(),
                    TasksCount = ws.Projects.SelectMany(p => p.Tasks).Count(),
                    CreatedAt = ws.CreatedAt
                },
                CompletionPercentage = ws.Projects.SelectMany(p => p.Tasks).Count() == 0 ? 0 : 
                (double)ws.Projects.SelectMany(p => p.Tasks).
                Count(t => t.TaskStatus == ProjectTaskStatus.Done) / 
                ws.Projects.SelectMany(p => p.Tasks).Count() * 100,
                Members = ws.WorkSpaceUsers.Select(wu => new MemberDto
                {
                    Id = wu.Id,
                    FullName = wu.User.FirstName + " " + wu.User.LastName
                }),

                ProjectNames = ws.Projects.Select(p => p.Name)
            }).FirstOrDefaultAsync();

            return result;
        }
    }
}
