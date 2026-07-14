using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Notifications.Command.GetNotificationByIdAndUserId
{
    public class GetNotificationByIdAndUserIdQueryValidator : AbstractValidator<GetNotificationByIdAndUserIdQuery>
    {
        public GetNotificationByIdAndUserIdQueryValidator()
        {
            RuleFor(p => p.NotificationId)
             .NotEmpty().WithMessage("NotificationId is required.");

            RuleFor(p => p.UserId)
                .NotEmpty().WithMessage("UserId is required.");

        }
    }
}
