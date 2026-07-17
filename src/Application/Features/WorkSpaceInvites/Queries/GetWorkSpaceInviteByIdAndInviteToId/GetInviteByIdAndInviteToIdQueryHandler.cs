using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaceInvites.Queries.GetInviteByIdAndInviteToId
{
    public class GetInviteByIdAndInviteToIdQueryHandler(IUnitOfWork unitOfWork
        , ILogger<GetInviteByIdAndInviteToIdQueryHandler> logger)
        : IRequestHandler<GetInviteByIdAndInviteToIdQuery, ErrorOr<WorkSpaceInviteDto>>
    {
        public async Task<ErrorOr<WorkSpaceInviteDto>> Handle(GetInviteByIdAndInviteToIdQuery request, CancellationToken cancellationToken)
        {
           var workSpaceInviteId = request.WorkSpaceInviteId;
           var inviteToId = request.InviteToId;

            logger.LogInformation("Starting getting workSpaceInviteId with id {WorkSpaceInviteId} " +
                "for inviteTo user with id {InviteToId}", workSpaceInviteId, inviteToId);


            logger.LogInformation("Getting workSpaceInvite with id {WorkSpaceInviteId} for inviteTo user with id {InviteToId}", workSpaceInviteId, inviteToId);
            var workSpaceInvite = await unitOfWork.WorkSpaceInviteRepository.GetWorkSpaceInviteByIdAndInviteToIdAsync(workSpaceInviteId, inviteToId);
            if(workSpaceInvite is null)
            {
                logger.LogWarning("WorkSpaceInvite with id {WorkSpaceInviteId} not found for inviteTo user with id {InviteToId}", workSpaceInviteId, inviteToId);
                return WorkSpaceInviteErrors.WorkSpaceInviteNotFoundByIdAndInvitedToId(workSpaceInviteId, inviteToId);
            }

            logger.LogInformation("WorkSpaceInvite with id {WorkSpaceInviteId} found for inviteTo user with id {InviteToId}", workSpaceInviteId, inviteToId);
            return workSpaceInvite.Adapt<WorkSpaceInviteDto>();
        }
    }
}
