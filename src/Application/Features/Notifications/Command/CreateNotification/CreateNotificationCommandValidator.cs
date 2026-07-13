using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Notifications.Command.CreateNotification
{
    public class CreateNotificationCommandValidator : AbstractValidator<CreateNotificationCommand>
    {
        public CreateNotificationCommandValidator()
        {
            RuleFor(p => p.CreateNotificationDto.NotifyToId)
                .NotEmpty().WithMessage("NotifyToId is required.");

            RuleFor(p => p.CreateNotificationDto.Title)
               .NotEmpty().WithMessage("Title is required.");

            RuleFor(p => p.CreateNotificationDto.Message)
               .NotEmpty().WithMessage("Message is required.");

            RuleFor(p => p.CreateNotificationDto.NotificationTypeId)
            .NotEmpty().WithMessage("NotificationTypeId is required.");
        }
    }
}
