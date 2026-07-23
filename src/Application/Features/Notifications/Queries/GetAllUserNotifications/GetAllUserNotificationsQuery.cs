using Application.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Notifications.Command.GetAllUserNotifications
{
    public sealed record GetAllUserNotificationsQuery(string UserId,PaginationRequestDto PaginationRequestDto) : IRequest<ErrorOr<PaginationResultDto<NotificationDto>>>;
   
}
