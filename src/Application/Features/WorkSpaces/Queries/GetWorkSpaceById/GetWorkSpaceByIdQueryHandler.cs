using Application.Common.Dtos;
using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Domain.Common.Enums;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaces.commands.GetWorkSpaceById
{
    public class GetWorkSpaceByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetWorkSpaceByIdQueryHandler> logger
        ) : IRequestHandler<GetWorkSpaceByIdQuery, ErrorOr<WorkSpaceDto>>

    {
        public async Task<ErrorOr<WorkSpaceDto>> Handle(GetWorkSpaceByIdQuery request, CancellationToken cancellationToken)
        {
            var workSpaceId = request.WorkSpaceId;
            logger.LogInformation("Starting get workspace by Id {WorkSPaceId}", workSpaceId);


            logger.LogInformation("Getting workspace by Id {WorkSPaceId}", workSpaceId);
            var workSpace = await unitOfWork.WorkSpaceRepository.GetByIdAsync(workSpaceId);

            if (workSpace is null)
            {
                logger.LogInformation("Workspace by Id {WorkSPaceId} not found", workSpaceId);
                return WorkSpaceErrors.WorkSpaceNotFoundById(workSpaceId);
            }

            logger.LogInformation("Getting workspace by Id {WorkSPaceId} successfully", workSpaceId);
            return workSpace.Adapt<WorkSpaceDto>();
        }

    }
}

