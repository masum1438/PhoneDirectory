using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneDirectoryApi.Models.Dtos;
using PhoneDirectoryApi.Services;

namespace PhoneDirectoryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(registerDto);

            if (result.Succeeded)
            {
                _logger.LogInformation($"User registered: {registerDto.Email}");
                return Ok(new { Message = "User registered successfully as Client." });
            }

            return BadRequest(result.Errors);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUserWithRole([FromBody] RegisterDto registerDto, [FromQuery] string role)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(role))
                return BadRequest(new { Message = "Role is required" });

            var result = await _authService.CreateUserWithRoleAsync(registerDto, role);

            if (result.Succeeded)
            {
                _logger.LogInformation($"Admin created user: {registerDto.Email} with role {role}");
                return Ok(new { Message = $"User created with role {role}." });
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(loginDto);

            if (result.Succeeded)
            {
                _logger.LogInformation($"User logged in: {loginDto.Email}");
                return Ok(new { Token = result.Token, Message = "Login successful." });
            }
            return Unauthorized(new { Message = "Invalid login attempt." });
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            _logger.LogInformation("User logged out.");
            return Ok(new { Message = "Logged out successfully." });
        }
    }
}
