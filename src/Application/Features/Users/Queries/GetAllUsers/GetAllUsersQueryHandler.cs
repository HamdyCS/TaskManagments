using Application.Common.Dtos;
using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Users.Queries.GetAllUsers
{
    public class GetAllUsersQueryHandler(IUnitOfWork unitOfWork,ILogger<GetAllUsersQueryHandler> logger) : IRequestHandler<GetAllUsersQuery, ErrorOr<PaginationResultDto<UserDto>>>
    {
        public async Task<ErrorOr<PaginationResultDto<UserDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting get all users");

            logger.LogInformation("Getting all users from db");
            var paginationResult = await unitOfWork.userRepository.GetAllUsers(request.PaginationRequestDto.PageNumber, request.PaginationRequestDto.PageSize);
            var paginationResultDto = paginationResult.Adapt<PaginationResultDto<UserDto>>();

            return paginationResultDto;
           
        }
    }
}
