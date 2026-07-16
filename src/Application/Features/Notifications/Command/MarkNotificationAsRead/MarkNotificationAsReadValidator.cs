using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Notifications.Command.ReadNotification
{
    public class MarkNotificationAsReadValidator : AbstractValidator<MarkNotificationAsReadCommand>
    {
        public MarkNotificationAsReadValidator()
        {
            RuleFor(p => p.NotificationId)
            .NotEmpty().WithMessage("NotificationId is required.");

            RuleFor(p => p.NotifyToId)
                .NotEmpty().WithMessage("NotifyToId is required.");

        }
    }
}
