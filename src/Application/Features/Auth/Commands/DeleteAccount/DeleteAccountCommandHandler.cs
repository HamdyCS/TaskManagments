using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Application.Features.Auth.Commands.DeleteAccount
{
    public class DeleteAccountCommandHandler(IUnitOfWork unitOfWork,
        ILogger<DeleteAccountCommandHandler> logger) : IRequestHandler<DeleteAccountCommand, ErrorOr<bool>>
    {
        public async Task<ErrorOr<bool>> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting Delete User with Id {UserId}", request.UserId);

            logger.LogInformation("Getting user with Id {UserId}", request.UserId);
            var user = await unitOfWork.UserRepository.GetByIdAsync(request.UserId);
            if (user is null)
            {
                logger.LogWarning("User with Id {UserId} not found", request.UserId);
                return UserErrors.UserNotFoundById(request.UserId);
            }

            logger.LogInformation("Verifying delete account token for user with Id {UserId}", request.UserId);
            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.DeleteAccountDto.Token));

            var isVerified = await unitOfWork.UserRepository.VerifyDeleteAccountTokenAsync(user, decodedToken);

            if (!isVerified)
            {
                logger.LogWarning("Failed to verify delete account token for user with Id {UserId}", request.UserId);
                return UserErrors.DeleteUserFailed(request.UserId);
            }

            logger.LogInformation("Deleting user with Id {UserId}", request.UserId);
            var isUserDeleted = await unitOfWork.UserRepository.DeleteAsync(user);

            if (!isUserDeleted)
            {
                logger.LogWarning("Failed to Delete user with Id {UserId}", request.UserId);
                return UserErrors.DeleteUserFailed(request.UserId);
            }

            logger.LogInformation("Deleted user with Id {UserId} successfully", request.UserId);
            return true;
        }
    }
}
