using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZenFlow.UserService.DTOs;
using ZenFlow.UserService.Services;

namespace ZenFlow.UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("health")]
        public IActionResult HealthCheck()
        {
            return Ok(new { status = "User Service is running", timestamp = DateTime.UtcNow });
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register(CreateUserDto createUserDto)
        {
            try
            {
                var result = await _userService.RegisterAsync(createUserDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
        {
            try
            {
                var result = await _userService.LoginAsync(loginDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            try
            {
                var result = await _userService.GetUserByIdAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<ActionResult<UserProfileDto>> GetUserProfile()
        {
            try
            {
                // Get user ID from JWT token
                var userId = int.Parse(User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value ?? "0");
                
                if (userId == 0)
                {
                    return Unauthorized();
                }

                var result = await _userService.GetUserProfileAsync(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("profile")]
        [Authorize]
        public async Task<ActionResult<UserProfileDto>> CreateOrUpdateUserProfile(UserProfileDto profileDto)
        {
            try
            {
                // Get user ID from JWT token
                var userId = int.Parse(User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value ?? "0");
                
                if (userId == 0)
                {
                    return Unauthorized();
                }

                var result = await _userService.CreateOrUpdateUserProfileAsync(userId, profileDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}