using Application.Common.Dtos;
using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Domain.Common.Enums;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaces.commands.GetAllUserWorkSpaces
{
    public class GetAllUserWorkSpacesQueryHandler(IUnitOfWork unitOfWork, ILogger<GetAllUserWorkSpacesQueryHandler> logger
        ) : IRequestHandler<GetAllUserWorkSpacesQuery, ErrorOr<PaginationResultDto<WorkSpaceDto>>>

    {
        public async Task<ErrorOr<PaginationResultDto<WorkSpaceDto>>> Handle(GetAllUserWorkSpacesQuery request, CancellationToken cancellationToken)
        {
            var pageSize = request.PaginationRequestDto.PageSize;
            var pageNumber = request.PaginationRequestDto.PageNumber;
            var userId = request.UserId;

            logger.LogInformation("Starting Get all workspaces for user with id {UserId}", userId);


            logger.LogInformation("Getting all workspaces for user with id {UserId}", userId);
            var workSpaces = await unitOfWork.WorkSpaceRepository
                .GetAllUserWorkSpaces(userId, pageNumber, pageSize);

            logger.LogInformation("Getting all workspaces for user with id {UserId} successfully", userId);
            return workSpaces.Adapt<PaginationResultDto<WorkSpaceDto>>();
        }

    }
}

