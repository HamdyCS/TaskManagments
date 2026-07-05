using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Users.Queries.GetUserById
{
    public class UpdateUserCommandHandlerHandler(IUnitOfWork unitOfWork, ILogger<UpdateUserCommandHandlerHandler> logger) : IRequestHandler<UpdateUserCommand, ErrorOr<UserDto>>
    {
        public async Task<ErrorOr<UserDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting Update user with id {UserId}", request.UserId);

            //get user
            var user = await unitOfWork.userRepository.GetByIdAsync(request.UserId);
            if (user is null)
            {
                logger.LogWarning("User with id {UserId} not found", request.UserId);
                return UserErrors.UserNotFoundById(request.UserId);
            }

            logger.LogInformation("User with id {UserId} found", request.UserId);


            logger.LogInformation("Updating User object from UpdateUserDto for user with id {UserId}", request.UserId);
            request.UpdateUserDto.Adapt(user);

            //update user
            logger.LogInformation("Updating user with id {UserId}", request.UserId);
            unitOfWork.userRepository.UpdateUser(user);


            //save changes
            logger.LogInformation("Saving changes for user with id {UserId}", request.UserId);
            var updateUserRowsAffected = await unitOfWork.SaveChangesAsync(cancellationToken);

            if(updateUserRowsAffected == 0)
            {
                logger.LogWarning("Failed to update user with id {UserId}", request.UserId);
                return UserErrors.UpdateUserFailed(request.UserId);
            }

            logger.LogInformation("User with id {UserId} updated successfully", request.UserId);
            

            //update user dto
            logger.LogInformation("Mapping user with id {UserId} to UserDto", request.UserId);
            var userDto = user.Adapt<UserDto>();

            return userDto;
        }
    }
}
