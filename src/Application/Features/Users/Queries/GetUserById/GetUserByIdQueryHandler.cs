using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Users.Queries.GetUserById
{
    public class GetUserByIdQueryHandler(IUnitOfWork unitOfWork,ILogger<GetUserByIdQueryHandler> logger) : IRequestHandler<GetUserByIdQuery, ErrorOr<UserDto>>
    {
        public async Task<ErrorOr<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting Get user by Id {UserId}", request.userId);

            //get user
            var user = await unitOfWork.userRepository.GetByIdAsync(request.userId);
            if(user is null)
            {
                logger.LogWarning("User with id {UserId} not found", request.userId);
                return UserErrors.UserNotFoundById(request.userId);
            }

            logger.LogInformation("User with id {UserId} found", request.userId);
            

            logger.LogInformation("Mapping User to UserDto for user with id {UserId}", request.userId);
            var userDto = user.Adapt<UserDto>();
            return userDto;
        }
    }
}
