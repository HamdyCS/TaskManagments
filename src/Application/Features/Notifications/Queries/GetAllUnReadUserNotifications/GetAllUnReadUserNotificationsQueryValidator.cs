using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Notifications.Command.GetAllUnReadUserNotifications
{
    public class GetAllUnReadUserNotificationsQueryValidator : AbstractValidator<GetAllUnReadUserNotificationsQuery>
    {
        public GetAllUnReadUserNotificationsQueryValidator()
        {
            RuleFor(p => p.UserId)
                .NotEmpty().WithMessage("UserId is required.");

        }
    }
}
