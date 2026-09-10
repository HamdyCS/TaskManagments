using Application.Common.Emails;
using Application.Common.Errors;
using Application.Common.Interfaces.Channels;
using Application.Common.Interfaces.Repositories;
using Domain.Common.Enums;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Application.Features.Auth.Commands.SendDeleteAccountEmail
{
    public class SendDeleteAccountEmailCommandHandler(IUnitOfWork unitOfWork,
        IDeleteAccountEmailQueue deleteAccountEmailQueue, IConfiguration configuration,
        ILogger<SendDeleteAccountEmailCommandHandler> logger) : IRequestHandler<SendDeleteAccountEmailCommand, ErrorOr<bool>>
    {
        public async Task<ErrorOr<bool>> Handle(SendDeleteAccountEmailCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Started sending delete account email for user with id {UserId}", request.UserId);

            logger.LogInformation("Getting user with id {UserId}", request.UserId);
            var user = await unitOfWork.UserRepository.GetByIdAsync(request.UserId);

            if (user is null)
            {
                logger.LogWarning("User with id {UserId} not found", request.UserId);
                return UserErrors.UserNotFoundById(request.UserId);
            }

            logger.LogInformation("Generating delete account token for user with id {UserId}", request.UserId);
            var token = await unitOfWork.UserRepository.GenerateDeleteAccountTokenAsync(user);

            if (token is null)
            {
                logger.LogWarning("Delete account token not generated for user with id {UserId}", request.UserId);
                return UserErrors.DeleteUserFailed(request.UserId);
            }

            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var dashboardPath = user.RoleId == (int)Role.Admin ? "admin/dashboard" : "dashboard";
            var path = configuration["settings:frontendUrl"] + '/' + dashboardPath + "/account/confirm-delete?token=" + encodedToken;

            logger.LogInformation("Adding delete account email to queue for user with id {UserId}", request.UserId);
            await deleteAccountEmailQueue.EnqueueAsync(new DeleteAccountEmailContent
            {
                FullName = user.FullName,
                To = user.Email!,
                Url = path
            });

            logger.LogInformation("Delete account email sent for user with id {UserId} successfully", request.UserId);
            return true;
        }
    }
}
