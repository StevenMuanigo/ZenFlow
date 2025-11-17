using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZenFlow.ReminderService.DTOs;
using ZenFlow.ReminderService.Services;

namespace ZenFlow.ReminderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReminderController : ControllerBase
    {
        private readonly IReminderService _reminderService;

        public ReminderController(IReminderService reminderService)
        {
            _reminderService = reminderService;
        }

        [HttpGet("health")]
        public IActionResult HealthCheck()
        {
            return Ok(new { status = "Reminder Service is running", timestamp = DateTime.UtcNow });
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ReminderDto>> CreateReminder(CreateReminderDto reminderDto)
        {
            try
            {
                var reminder = await _reminderService.CreateReminderAsync(reminderDto);
                return CreatedAtAction(nameof(GetReminder), new { id = reminder.Id }, reminder);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ReminderDto>> GetReminder(string id)
        {
            try
            {
                var reminder = await _reminderService.GetReminderByIdAsync(id);
                if (reminder == null)
                {
                    return NotFound();
                }
                return Ok(reminder);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("user/{userId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ReminderDto>>> GetRemindersByUser(int userId)
        {
            try
            {
                var reminders = await _reminderService.GetRemindersByUserIdAsync(userId);
                return Ok(reminders);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("user/{userId}/active")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ReminderDto>>> GetActiveRemindersByUser(int userId)
        {
            try
            {
                var reminders = await _reminderService.GetActiveRemindersByUserIdAsync(userId);
                return Ok(reminders);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<ReminderDto>> UpdateReminder(string id, ReminderDto reminderDto)
        {
            try
            {
                var reminder = await _reminderService.UpdateReminderAsync(id, reminderDto);
                if (reminder == null)
                {
                    return NotFound();
                }
                return Ok(reminder);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<bool>> DeleteReminder(string id)
        {
            try
            {
                var result = await _reminderService.DeleteReminderAsync(id);
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
    }
}