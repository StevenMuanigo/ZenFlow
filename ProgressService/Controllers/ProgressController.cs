using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZenFlow.ProgressService.DTOs;
using ZenFlow.ProgressService.Services;

namespace ZenFlow.ProgressService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProgressController : ControllerBase
    {
        private readonly IProgressService _progressService;

        public ProgressController(IProgressService progressService)
        {
            _progressService = progressService;
        }

        [HttpGet("health")]
        public IActionResult HealthCheck()
        {
            return Ok(new { status = "Progress Service is running", timestamp = DateTime.UtcNow });
        }

        [HttpPost("records")]
        [Authorize]
        public async Task<ActionResult<ProgressRecordDto>> CreateProgressRecord(CreateProgressRecordDto recordDto)
        {
            try
            {
                var record = await _progressService.CreateProgressRecordAsync(recordDto);
                return CreatedAtAction(nameof(GetProgressRecord), new { id = record.Id }, record);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("records/{id}")]
        [Authorize]
        public async Task<ActionResult<ProgressRecordDto>> GetProgressRecord(string id)
        {
            try
            {
                var record = await _progressService.GetProgressRecordByIdAsync(id);
                if (record == null)
                {
                    return NotFound();
                }
                return Ok(record);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("users/{userId}/records")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ProgressRecordDto>>> GetProgressRecordsByUser(int userId)
        {
            try
            {
                var records = await _progressService.GetProgressRecordsByUserIdAsync(userId);
                return Ok(records);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("users/{userId}/records/range")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ProgressRecordDto>>> GetProgressRecordsByUserAndDateRange(
            int userId, 
            [FromQuery] DateTime startDate, 
            [FromQuery] DateTime endDate)
        {
            try
            {
                var records = await _progressService.GetProgressRecordsByUserIdAndDateRangeAsync(userId, startDate, endDate);
                return Ok(records);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("records/{id}")]
        [Authorize]
        public async Task<ActionResult<ProgressRecordDto>> UpdateProgressRecord(string id, ProgressRecordDto recordDto)
        {
            try
            {
                var record = await _progressService.UpdateProgressRecordAsync(id, recordDto);
                if (record == null)
                {
                    return NotFound();
                }
                return Ok(record);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("records/{id}")]
        [Authorize]
        public async Task<ActionResult<bool>> DeleteProgressRecord(string id)
        {
            try
            {
                var result = await _progressService.DeleteProgressRecordAsync(id);
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

        [HttpGet("users/{userId}/summary")]
        [Authorize]
        public async Task<ActionResult<ProgressSummaryDto>> GetProgressSummary(int userId)
        {
            try
            {
                var summary = await _progressService.GetProgressSummaryAsync(userId);
                return Ok(summary);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("users/{userId}/chart")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ProgressChartDto>>> GetProgressChartData(
            int userId,
            [FromQuery] string metricType,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            try
            {
                var chartData = await _progressService.GetProgressChartDataAsync(userId, metricType, startDate, endDate);
                return Ok(chartData);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}