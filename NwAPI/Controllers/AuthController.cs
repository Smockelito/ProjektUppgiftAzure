using Data.Dto;
using Microsoft.AspNetCore.Mvc;
using NwAPI.Services;

namespace NwAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        // POST api/Auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("Användare skapad.");
        }

        // POST api/Auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var token = await _authService.LoginAsync(dto);
            if (token == null)
                return Unauthorized("Felaktig e-post eller lösenord.");

            return Ok(new { token });
        }
    }
}
