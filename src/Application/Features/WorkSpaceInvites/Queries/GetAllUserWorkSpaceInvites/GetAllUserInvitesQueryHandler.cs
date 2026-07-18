using Application.Common.Dtos;
using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaceInvites.Queries.GetAllUserInvites
{
    public class GetAllUserInvitesQueryHandler(IUnitOfWork unitOfWork
        , ILogger<GetAllUserInvitesQueryHandler> logger)
        : IRequestHandler<GetAllUserInvitesQuery, ErrorOr<PaginationResultDto<WorkSpaceInviteDto>>>
    {
        public async Task<ErrorOr<PaginationResultDto<WorkSpaceInviteDto>>> Handle(GetAllUserInvitesQuery request, CancellationToken cancellationToken)
        {
            var inviteToId = request.InviteToId;

            logger.LogInformation("Starting getting workSpaceInvite for invite To user with id {InviteToId}"
               , inviteToId);


            logger.LogInformation("Getting workSpaceInvite for invite To user with id {InviteToId}", inviteToId);
            var workSpaceInvites = await unitOfWork.WorkSpaceInviteRepository.GetAllWorkSpaceInvitesByInviteToIdAsync(inviteToId,request.PaginationRequest.PageNumber,
                request.PaginationRequest.PageSize);
          

            logger.LogInformation("Got workSpaceInvite for invite To user with id {InviteToId} successfully", inviteToId);
            return workSpaceInvites.Adapt<PaginationResultDto<WorkSpaceInviteDto>>();
        }
    }
}
