using Application.Common.Dtos;
using Application.Common.Interfaces.Repositories;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaceUsers.queries.GetAllWorkSpaceUsers
{
    public class GetAllWorkSpaceUsersQueryHandler(IUnitOfWork unitOfWork,
        ILogger<GetAllWorkSpaceUsersQueryHandler> logger) : IRequestHandler<GetAllWorkSpaceUsersQuery, ErrorOr<PaginationResultDto<WorkSpaceUserDto>>>
    {
        public async Task<ErrorOr<PaginationResultDto<WorkSpaceUserDto>>> Handle(GetAllWorkSpaceUsersQuery request, CancellationToken cancellationToken)
        {
            var workspaceId = request.WorkSpaceId;
            var pageNumber = request.PaginationRequestDto.PageNumber;
            var pageSize = request.PaginationRequestDto.PageSize;

            logger.LogInformation("Starting getting work space users for work space with id {WorkSpaceId}", workspaceId);

            logger.LogInformation("Getting work space users for work space with id {WorkSpaceId}", workspaceId);
            var workSpaceUsers = await unitOfWork.WorkSpaceUserRepository.GetWorkSpaceUsersAsync(workspaceId, pageNumber, pageSize);

            logger.LogInformation("Got work space users for work space with id {WorkSpaceId} successfully", workspaceId);
            return workSpaceUsers.Adapt<PaginationResultDto<WorkSpaceUserDto>>();
        }
    }
}
