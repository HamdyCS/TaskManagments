using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Notifications.Command.GetAllUserNotifications
{
    public class GetAllUserNotificationsQueryValidator : AbstractValidator<GetAllUserNotificationsQuery>
    {
        public GetAllUserNotificationsQueryValidator()
        {
            RuleFor(p => p.UserId)
                .NotEmpty().WithMessage("UserId is required.");

        }
    }
}
