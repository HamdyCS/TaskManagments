using Application.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Notifications.Command.GetAllUnReadUserNotifications
{
    public sealed record GetAllUnReadUserNotificationsQuery(string UserId,PaginationRequestDto PaginationRequestDto) : IRequest<ErrorOr<PaginationResultDto<NotificationDto>>>;
   
}
