using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZenFlow.WorkoutService.DTOs;
using ZenFlow.WorkoutService.Services;

namespace ZenFlow.WorkoutService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkoutController : ControllerBase
    {
        private readonly IWorkoutService _workoutService;

        public WorkoutController(IWorkoutService workoutService)
        {
            _workoutService = workoutService;
        }

        [HttpGet("health")]
        public IActionResult HealthCheck()
        {
            return Ok(new { status = "Workout Service is running", timestamp = DateTime.UtcNow });
        }

        // Workout endpoints
        [HttpPost("workouts")]
        [Authorize]
        public async Task<ActionResult<WorkoutDto>> CreateWorkout(CreateWorkoutDto workoutDto)
        {
            try
            {
                var workout = await _workoutService.CreateWorkoutAsync(workoutDto);
                return CreatedAtAction(nameof(GetWorkout), new { id = workout.Id }, workout);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("workouts/{id}")]
        [Authorize]
        public async Task<ActionResult<WorkoutDto>> GetWorkout(string id)
        {
            try
            {
                var workout = await _workoutService.GetWorkoutByIdAsync(id);
                if (workout == null)
                {
                    return NotFound();
                }
                return Ok(workout);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("users/{userId}/workouts")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<WorkoutDto>>> GetWorkoutsByUser(int userId)
        {
            try
            {
                var workouts = await _workoutService.GetWorkoutsByUserIdAsync(userId);
                return Ok(workouts);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("workouts/{id}")]
        [Authorize]
        public async Task<ActionResult<WorkoutDto>> UpdateWorkout(string id, WorkoutDto workoutDto)
        {
            try
            {
                var workout = await _workoutService.UpdateWorkoutAsync(id, workoutDto);
                if (workout == null)
                {
                    return NotFound();
                }
                return Ok(workout);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("workouts/{id}")]
        [Authorize]
        public async Task<ActionResult<bool>> DeleteWorkout(string id)
        {
            try
            {
                var result = await _workoutService.DeleteWorkoutAsync(id);
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

        // Exercise endpoints
        [HttpPost("exercises")]
        [Authorize]
        public async Task<ActionResult<ExerciseDto>> CreateExercise(CreateExerciseDto exerciseDto)
        {
            try
            {
                var exercise = await _workoutService.CreateExerciseAsync(exerciseDto);
                return CreatedAtAction(nameof(GetExercise), new { id = exercise.Id }, exercise);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("exercises/{id}")]
        [Authorize]
        public async Task<ActionResult<ExerciseDto>> GetExercise(string id)
        {
            try
            {
                var exercise = await _workoutService.GetExerciseByIdAsync(id);
                if (exercise == null)
                {
                    return NotFound();
                }
                return Ok(exercise);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("exercises")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ExerciseDto>>> GetAllExercises()
        {
            try
            {
                var exercises = await _workoutService.GetAllExercisesAsync();
                return Ok(exercises);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("exercises/muscle-group/{muscleGroup}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ExerciseDto>>> GetExercisesByMuscleGroup(string muscleGroup)
        {
            try
            {
                var exercises = await _workoutService.GetExercisesByMuscleGroupAsync(muscleGroup);
                return Ok(exercises);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Workout session endpoints
        [HttpPost("sessions/start")]
        [Authorize]
        public async Task<ActionResult<WorkoutSessionDto>> StartWorkoutSession(string workoutId, int userId)
        {
            try
            {
                var session = await _workoutService.StartWorkoutSessionAsync(workoutId, userId);
                return Ok(session);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("sessions/{id}")]
        [Authorize]
        public async Task<ActionResult<WorkoutSessionDto>> UpdateWorkoutSession(string id, WorkoutSessionDto sessionDto)
        {
            try
            {
                var session = await _workoutService.UpdateWorkoutSessionAsync(id, sessionDto);
                if (session == null)
                {
                    return NotFound();
                }
                return Ok(session);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("sessions/{id}/complete")]
        [Authorize]
        public async Task<ActionResult<WorkoutSessionDto>> CompleteWorkoutSession(string id)
        {
            try
            {
                var session = await _workoutService.CompleteWorkoutSessionAsync(id);
                if (session == null)
                {
                    return NotFound();
                }
                return Ok(session);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("sessions/{id}")]
        [Authorize]
        public async Task<ActionResult<WorkoutSessionDto>> GetWorkoutSession(string id)
        {
            try
            {
                var session = await _workoutService.GetWorkoutSessionByIdAsync(id);
                if (session == null)
                {
                    return NotFound();
                }
                return Ok(session);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("users/{userId}/sessions")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<WorkoutSessionDto>>> GetWorkoutSessionsByUser(int userId)
        {
            try
            {
                var sessions = await _workoutService.GetWorkoutSessionsByUserIdAsync(userId);
                return Ok(sessions);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}