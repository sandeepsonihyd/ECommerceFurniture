using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ECommerceFurniture.WebAPI.DTOs;

namespace ECommerceFurniture.WebAPI.Services
{
    /// <summary>
    /// Service responsible for handling authentication operations including login, token validation, and JWT token generation.
    /// Implements simple username/password authentication where the username and password must match.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the AuthService class.
        /// </summary>
        /// <param name="configuration">Configuration instance for accessing application settings, particularly JWT settings.</param>
        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Authenticates a user with the provided login credentials.
        /// Uses simple validation where the username and password must be identical.
        /// </summary>
        /// <param name="loginRequest">The login request containing username and password.</param>
        /// <returns>A task that returns a LoginResponseDto indicating success or failure with appropriate messaging.</returns>
        public Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequest)
        {
            // Validate that both username and password are provided
            if (string.IsNullOrWhiteSpace(loginRequest.Username) || 
                string.IsNullOrWhiteSpace(loginRequest.Password))
            {
                            return Task.FromResult(new LoginResponseDto
                            {
                                Success = false,
                                Message = "Username and password are required."
                            });
            }

            // Simple authentication: username and password must match
            // Note: This is a basic implementation for demo purposes
            if (loginRequest.Username != loginRequest.Password)
            {
                            return Task.FromResult(new LoginResponseDto
                            {
                                Success = false,
                                Message = "Invalid username or password."
                            });
            }

            // Generate JWT token for successful authentication
            var token = GenerateJwtToken(loginRequest.Username);

            return Task.FromResult(new LoginResponseDto
            {
                Success = true,
                Token = token,
                Username = loginRequest.Username,
                Message = "Login successful."
            });
        }

        /// <summary>
        /// Validates the provided JWT token to ensure it's properly formatted, signed, and not expired.
        /// </summary>
        /// <param name="token">The JWT token string to validate.</param>
        /// <returns>A task that returns true if the token is valid, false otherwise.</returns>
        public Task<bool> ValidateTokenAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return Task.FromResult(false);

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                // Get the JWT key from configuration, with fallback to default key
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? "YourSuperSecretKey12345678901234567890");

                // Validate the token against our security parameters
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"] ?? "ECommerceFurniture",
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"] ?? "ECommerceFurnitureUsers",
                    ClockSkew = TimeSpan.Zero // No tolerance for clock differences
                }, out SecurityToken validatedToken);

                return Task.FromResult(true);
            }
            catch
            {
                // Token validation failed - could be expired, malformed, or improperly signed
                return Task.FromResult(false);
            }
        }

        /// <summary>
        /// Extracts the username from a JWT token without validating the token's signature or expiration.
        /// </summary>
        /// <param name="token">The JWT token string to extract the username from.</param>
        /// <returns>The username stored in the token's claims, or null if extraction fails.</returns>
        public string? GetUsernameFromToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null;

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                // Extract the username from the Name claim
                return jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            }
            catch
            {
                // Token parsing failed - token may be malformed
                return null;
            }
        }

        /// <summary>
        /// Generates a new JWT token for the specified username.
        /// The token includes user claims and is configured with issuer, audience, and expiration settings.
        /// </summary>
        /// <param name="username">The username to include in the token claims.</param>
        /// <returns>A signed JWT token string.</returns>
        private string GenerateJwtToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            // Get the JWT key from configuration, with fallback to default key
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? "YourSuperSecretKey12345678901234567890");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // Add user claims to the token
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.NameIdentifier, username)
                }),
                Expires = DateTime.UtcNow.AddHours(24), // Token expires in 24 hours
                Issuer = _configuration["Jwt:Issuer"] ?? "ECommerceFurniture",
                Audience = _configuration["Jwt:Audience"] ?? "ECommerceFurnitureUsers",
                // Sign the token with HMAC SHA256
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            // Create and return the JWT token
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
} 