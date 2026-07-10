using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Application.Features.Auth.Commands.VerifyOtp;
using Domain.Common.Enums;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.DeleteUser
{
    public class DeleteUserCommandHandler(IUnitOfWork unitOfWork, IMediator mediator
        , ILogger<DeleteUserCommandHandler> logger) : IRequestHandler<DeleteUserCommand, ErrorOr<bool>>
    {
        public async Task<ErrorOr<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var userId = request.UserId;
            var deletesBy = request.DeletesBy;

            logger.LogInformation("Starting Delete User with Id {UserId} By {DeletesBy}", userId, deletesBy);

            if(userId == deletesBy)
            {
                logger.LogWarning("User with Id {UserId} can not delete itself", userId);
                return UserErrors.UserCanNotDeleteItself(userId);
            }


            logger.LogInformation("Getting user with Id {UserId}", userId);
            var user = await unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user is null)
            {
                logger.LogWarning("User with Id {UserId} not found", userId);
                return UserErrors.UserNotFoundById(userId);
            }

            //delete user

            logger.LogInformation("Deleting user with Id {UserId} by {DeletesBy}", userId, deletesBy);
            var isUserDeleted = await unitOfWork.UserRepository.DeleteAsync(user);

            if (!isUserDeleted)
            {
                logger.LogWarning("Failed to Delete user with Id {UserId}", userId);
                return UserErrors.DeleteUserFailed(userId);
            }

            logger.LogInformation("Deleted user with Id {UserId} successfully by {DeletesBy}", userId, deletesBy);

            return true;
        }
    }
}
