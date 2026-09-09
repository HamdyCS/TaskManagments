using Application.Common.Dtos;
using Application.Common.Interfaces.Repositories;
using Mapster;
using Microsoft.Extensions.Logging;

namespace Application.Features.Users.Queries.GetAllRegularUsers
{
    public class GetAllRegularUsersQueryHandler(IUnitOfWork unitOfWork, ILogger<GetAllRegularUsersQueryHandler> logger) : IRequestHandler<GetAllRegularUsersQuery, ErrorOr<PaginationResultDto<UserDto>>>
    {
        public async Task<ErrorOr<PaginationResultDto<UserDto>>> Handle(GetAllRegularUsersQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting get all regular users");

            logger.LogInformation("Getting all regular users from db");
            var paginationResult = await unitOfWork.UserRepository.GetAllRegularUsersAsync(request.PaginationRequestDto.PageNumber, request.PaginationRequestDto.PageSize);

            logger.LogInformation("Got all regular users from db successfully");
            var paginationResultDto = paginationResult.Adapt<PaginationResultDto<UserDto>>();

            return paginationResultDto;
        }
    }
}
