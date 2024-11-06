using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TicketingSystem.Dtos;
using TicketingSystem.Services.Interface;

namespace TicketingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.Register(request);
            if (!result)
            {
                return BadRequest("User already exists.");
            }

            return Ok(new { message = "User registered successfully." });
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var loginResponse = await _authService.Login(request);
            if (loginResponse == null)
            {
                return Unauthorized(new { message = "Invalid username or password." });
            }

            return Ok(loginResponse);
        }

        // Validate token (optional)
        [HttpGet("validate-token")]
        [Authorize] // Protect this endpoint if needed
        public async Task<IActionResult> ValidateToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return BadRequest("Token cannot be null or empty.");
            }

            var isValid = await _authService.ValidateToken(token);
            return Ok(isValid);
        }

        [HttpDelete("{username}")]
        public async Task<IActionResult> DeleteUser(string username)
        {
            var result = await _authService.DeleteUser(username);
            if (result)
            {
                return NoContent(); // User deleted successfully
            }
            return NotFound(); // User not found
        }
    }
}
