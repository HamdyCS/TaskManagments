using Application.Common.Dtos;
using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Domain.Common.Enums;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaces.commands.GetAllWorkSpaces
{
    public class GetAllWorkSpacesQueryHandler(IUnitOfWork unitOfWork, ILogger<GetAllWorkSpacesQueryHandler> logger
        ) : IRequestHandler<GetAllWorkSpacesQuery, ErrorOr<PaginationResultDto<WorkSpaceDto>>>

    {
        public async Task<ErrorOr<PaginationResultDto<WorkSpaceDto>>> Handle(GetAllWorkSpacesQuery request, CancellationToken cancellationToken)
        {
            var pageSize = request.PaginationRequestDto.PageSize;
            var pageNumber = request.PaginationRequestDto.PageNumber;
            logger.LogInformation("Starting Get all workspaces");


            logger.LogInformation("Getting all workspaces");
            var workSpaces = await unitOfWork.WorkSpaceRepository.GetAllAsync(pageNumber, pageSize);

            logger.LogInformation("Getting all workspaces successfully");
            return workSpaces.Adapt<PaginationResultDto<WorkSpaceDto>>();
        }

    }
}

