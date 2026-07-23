using Application.Common.Dtos;
using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaceInvites.Queries.GetAllSendWorkSpaceInvites
{
    public class GetAllSendInvitesQueryHandler(IUnitOfWork unitOfWork
        , ILogger<GetAllSendInvitesQueryHandler> logger)
        : IRequestHandler<GetAllSendInvitesQuery, ErrorOr<PaginationResultDto<WorkSpaceInviteDto>>>
    {
        public async Task<ErrorOr<PaginationResultDto<WorkSpaceInviteDto>>> Handle(GetAllSendInvitesQuery request, CancellationToken cancellationToken)
        {
            var inviteById = request.InviteById;

            logger.LogInformation("Starting getting workSpaceInvite for invite by user with id {inviteById}"
               , inviteById);


            logger.LogInformation("Getting workSpaceInvite for invite by user with id {inviteById}", inviteById);
            var workSpaceInvites = await unitOfWork.WorkSpaceInviteRepository.GetAllWorkSpaceInvitesByInviteByIdAsync(inviteById,request.PaginationRequest.PageNumber,
                request.PaginationRequest.PageSize);
          

            logger.LogInformation("Got workSpaceInvites for invite by user with id {inviteById} successfully", inviteById);
            return workSpaceInvites.Adapt<PaginationResultDto<WorkSpaceInviteDto>>();
        }
    }
}
