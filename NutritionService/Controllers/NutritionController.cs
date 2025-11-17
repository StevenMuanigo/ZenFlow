using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZenFlow.NutritionService.DTOs;
using ZenFlow.NutritionService.Services;

namespace ZenFlow.NutritionService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NutritionController : ControllerBase
    {
        private readonly INutritionPlanService _nutritionPlanService;

        public NutritionController(INutritionPlanService nutritionPlanService)
        {
            _nutritionPlanService = nutritionPlanService;
        }

        [HttpGet("health")]
        public IActionResult HealthCheck()
        {
            return Ok(new { status = "Nutrition Service is running", timestamp = DateTime.UtcNow });
        }

        [HttpPost("plans")]
        [Authorize]
        public async Task<ActionResult<NutritionPlanDto>> CreateNutritionPlan(CreateNutritionPlanDto planDto)
        {
            try
            {
                var plan = await _nutritionPlanService.CreateNutritionPlanAsync(planDto);
                return CreatedAtAction(nameof(GetNutritionPlan), new { id = plan.Id }, plan);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("plans/user/{userId}")]
        [Authorize]
        public async Task<ActionResult<NutritionPlanDto>> GetNutritionPlan(int userId)
        {
            try
            {
                var plan = await _nutritionPlanService.GetNutritionPlanByUserIdAsync(userId);
                if (plan == null)
                {
                    return NotFound();
                }
                return Ok(plan);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("plans/{id}")]
        [Authorize]
        public async Task<ActionResult<NutritionPlanDto>> UpdateNutritionPlan(string id, NutritionPlanDto planDto)
        {
            try
            {
                var plan = await _nutritionPlanService.UpdateNutritionPlanAsync(id, planDto);
                if (plan == null)
                {
                    return NotFound();
                }
                return Ok(plan);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("plans/{id}")]
        [Authorize]
        public async Task<ActionResult<bool>> DeleteNutritionPlan(string id)
        {
            try
            {
                var result = await _nutritionPlanService.DeleteNutritionPlanAsync(id);
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

        [HttpPost("plans/generate")]
        [Authorize]
        public async Task<ActionResult<NutritionPlanDto>> GeneratePersonalizedPlan(int userId)
        {
            try
            {
                var plan = await _nutritionPlanService.GeneratePersonalizedPlanAsync(userId);
                return Ok(plan);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}