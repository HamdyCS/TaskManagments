using Application.Common.Dtos;
using Application.Common.Interfaces.Repositories;
using Mapster;
using Microsoft.Extensions.Logging;

namespace Application.Features.Users.Queries.GetAllAdminUsers
{
    public class GetAllAdminUsersQueryHandler(IUnitOfWork unitOfWork, ILogger<GetAllAdminUsersQueryHandler> logger) : IRequestHandler<GetAllAdminUsersQuery, ErrorOr<PaginationResultDto<UserDto>>>
    {
        public async Task<ErrorOr<PaginationResultDto<UserDto>>> Handle(GetAllAdminUsersQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting get all admin users");

            logger.LogInformation("Getting all admin users from db");
            var paginationResult = await unitOfWork.UserRepository.GetAllAdminUsersAsync(request.PaginationRequestDto.PageNumber, request.PaginationRequestDto.PageSize);

            logger.LogInformation("Got all admin users from db successfully");
            var paginationResultDto = paginationResult.Adapt<PaginationResultDto<UserDto>>();

            return paginationResultDto;
        }
    }
}
