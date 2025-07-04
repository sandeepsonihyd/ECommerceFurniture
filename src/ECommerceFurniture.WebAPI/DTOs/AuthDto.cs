namespace ECommerceFurniture.WebAPI.DTOs
{
    public class LoginRequestDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponseDto
    {
        public bool Success { get; set; }
        public string? Token { get; set; }
        public string? Username { get; set; }
        public string? Message { get; set; }
    }

    public class LogoutRequestDto
    {
        public string Username { get; set; } = string.Empty;
    }
} 