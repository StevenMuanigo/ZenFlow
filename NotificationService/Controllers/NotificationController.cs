using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZenFlow.NotificationService.DTOs;
using ZenFlow.NotificationService.Services;

namespace ZenFlow.NotificationService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("health")]
        public IActionResult HealthCheck()
        {
            return Ok(new { status = "Notification Service is running", timestamp = DateTime.UtcNow });
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<NotificationDto>> CreateNotification(CreateNotificationDto notificationDto)
        {
            try
            {
                var notification = await _notificationService.CreateNotificationAsync(notificationDto);
                return CreatedAtAction(nameof(GetNotification), new { id = notification.Id }, notification);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<NotificationDto>> GetNotification(string id)
        {
            try
            {
                var notification = await _notificationService.GetNotificationByIdAsync(id);
                if (notification == null)
                {
                    return NotFound();
                }
                return Ok(notification);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("user/{userId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<NotificationDto>>> GetNotificationsByUser(int userId)
        {
            try
            {
                var notifications = await _notificationService.GetNotificationsByUserIdAsync(userId);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("user/{userId}/unread")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<NotificationDto>>> GetUnreadNotificationsByUser(int userId)
        {
            try
            {
                var notifications = await _notificationService.GetUnreadNotificationsByUserIdAsync(userId);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}/read")]
        [Authorize]
        public async Task<ActionResult<NotificationDto>> MarkNotificationAsRead(string id)
        {
            try
            {
                var notification = await _notificationService.MarkNotificationAsReadAsync(id);
                if (notification == null)
                {
                    return NotFound();
                }
                return Ok(notification);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("user/{userId}/read-all")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<NotificationDto>>> MarkAllNotificationsAsRead(int userId)
        {
            try
            {
                var notifications = await _notificationService.MarkAllNotificationsAsReadAsync(userId);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<bool>> DeleteNotification(string id)
        {
            try
            {
                var result = await _notificationService.DeleteNotificationAsync(id);
                if (!result)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("user/{userId}/unread-count")]
        [Authorize]
        public async Task<ActionResult<int>> GetUnreadNotificationCount(int userId)
        {
            try
            {
                var count = await _notificationService.GetUnreadNotificationCountAsync(userId);
                return Ok(count);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}