using Microsoft.AspNetCore.Mvc;
using ECommerceFurniture.WebAPI.DTOs;
using ECommerceFurniture.WebAPI.Services;

namespace ECommerceFurniture.WebAPI.Controllers
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

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto loginRequest)
        {
            try
            {
                if (loginRequest == null)
                {
                    return BadRequest("Login request is required.");
                }

                var result = await _authService.LoginAsync(loginRequest);

                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return Unauthorized(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("validate")]
        public async Task<ActionResult<bool>> ValidateToken([FromHeader(Name = "Authorization")] string? authorization)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(authorization) || !authorization.StartsWith("Bearer "))
                {
                    return Unauthorized(false);
                }

                var token = authorization.Substring("Bearer ".Length);
                var isValid = await _authService.ValidateTokenAsync(token);

                return Ok(isValid);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("user")]
        public Task<ActionResult<string?>> GetCurrentUser([FromHeader(Name = "Authorization")] string? authorization)
        {
            try
            {
                            if (string.IsNullOrWhiteSpace(authorization) || !authorization.StartsWith("Bearer "))
            {
                return Task.FromResult<ActionResult<string?>>(Unauthorized());
            }

            var token = authorization.Substring("Bearer ".Length);
            var username = _authService.GetUsernameFromToken(token);

            if (username == null)
            {
                return Task.FromResult<ActionResult<string?>>(Unauthorized());
            }

                return Task.FromResult<ActionResult<string?>>(Ok(new { username }));
            }
            catch (Exception ex)
            {
                return Task.FromResult<ActionResult<string?>>(StatusCode(500, $"Internal server error: {ex.Message}"));
            }
        }
    }
} 