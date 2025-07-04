using ECommerceFurniture.WebAPI.DTOs;

namespace ECommerceFurniture.WebAPI.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequest);
        Task<bool> ValidateTokenAsync(string token);
        string? GetUsernameFromToken(string token);
    }
} 