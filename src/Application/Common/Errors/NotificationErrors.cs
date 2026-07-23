using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Errors
{
    public static class NotificationErrors
    {
        public static Error NotificationNotFoundById(long id) =>
            Error.NotFound("Notification_NotFound", $"Notification not found with id {id}");

        public static Error NotificationNotFoundByIdAndUserId(long id, string userId) =>
            Error.NotFound("Notification_NotFound", $"Notification not found with id {id} for user with id {userId}");

        public static Error CreateNotificationFailed(string userId) =>
            Error.Failure("Notification_CreateFailed", $"Failed create notification for user with id {userId}");

        public static Error UpdateNotificationToReadFailed(long id, string userId) =>
            Error.Failure("Notification_UpdateToReadFailed", $"Failed update notification with id {id} to read for user with id {userId}");

        public static Error NotificationAlreadyRead(long id, string userId) =>
            Error.Conflict("Notification_AlreadyRead", $"Notification with id {id} already read for user with id {userId}");
    }
}
