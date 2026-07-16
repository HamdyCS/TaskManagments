using Application.Common.Dtos;
using Application.Features.Notifications;
using Application.Features.Notifications.Command.GetAllUnReadUserNotifications;
using Application.Features.Notifications.Command.GetAllUserNotifications;
using Application.Features.Notifications.Command.GetNotificationByIdAndUserId;
using Application.Features.Notifications.Command.ReadNotification;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/notifications")]
    [ApiController]
    public class NotificationsController(IMediator mediator) : ControllerBase
    {
        [HttpGet("{id}", Name = "GetNotificationById")]
        public async Task<ActionResult<NotificationDto>> GetNotificationById([FromRoute] long id)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await mediator.Send(new GetNotificationByIdAndUserIdQuery(id, userId));

            return result.Match(value => Ok(value),
                errors => errors.ToProblemDetailsObjectResult());
        }

        [HttpGet("all", Name = "GetAllUserNotifications")]
        public async Task<ActionResult<PaginationResultDto<NotificationDto>>> GetAllUserNotifications([FromQuery] PaginationRequestDto paginationRequestDto)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await
                mediator.Send(new GetAllUserNotificationsQuery(userId, paginationRequestDto));

            return result.Match(value => Ok(value),
                errors => errors.ToProblemDetailsObjectResult());
        }

        [HttpGet("all/unread", Name = "GetAllUnReadUserNotifications")]
        public async Task<ActionResult<PaginationResultDto<NotificationDto>>> GetAllUnReadUserNotifications([FromQuery] PaginationRequestDto paginationRequestDto)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await
                mediator.Send(new GetAllUnReadUserNotificationsQuery(userId, paginationRequestDto));

            return result.Match(value => Ok(value),
                errors => errors.ToProblemDetailsObjectResult());
        }

        [HttpPut("{id}/read", Name = "MarkNotificationAsRead")]
        public async Task<IActionResult> MarkNotificationAsRead([FromRoute] long id)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await mediator.Send(new MarkNotificationAsReadCommand(id, userId));
            return result.Match<IActionResult>(value => NoContent(),
                            errors => errors.ToProblemDetailsObjectResult());
        }
    }
}
