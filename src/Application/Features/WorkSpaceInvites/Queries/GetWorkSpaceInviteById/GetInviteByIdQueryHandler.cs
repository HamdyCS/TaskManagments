using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaceInvites.Queries.GetWorkSpaceInviteById
{
    public class GetInviteByIdQueryHandler(IUnitOfWork unitOfWork
        , ILogger<GetInviteByIdQueryHandler> logger)
        : IRequestHandler<GetInviteByIdQuery, ErrorOr<WorkSpaceInviteDto>>
    {
        public async Task<ErrorOr<WorkSpaceInviteDto>> Handle(GetInviteByIdQuery request, CancellationToken cancellationToken)
        {
           var workSpaceInviteId = request.WorkSpaceInviteId;

            logger.LogInformation("Starting getting workSpaceInviteId with id {WorkSpaceInviteId} " , workSpaceInviteId);


            logger.LogInformation("Getting workSpaceInvite with id {WorkSpaceInviteId} ", workSpaceInviteId);
            var workSpaceInvite = await unitOfWork.WorkSpaceInviteRepository.GetByIdAsync(workSpaceInviteId);
            if(workSpaceInvite is null)
            {
                logger.LogWarning("WorkSpaceInvite with id {WorkSpaceInviteId} not found ", workSpaceInviteId);
                return WorkSpaceInviteErrors.WorkSpaceInviteNotFoundById(workSpaceInviteId);
            }

            logger.LogInformation("WorkSpaceInvite with id {WorkSpaceInviteId} found", workSpaceInviteId);
            return workSpaceInvite.Adapt<WorkSpaceInviteDto>();
        }
    }
}
