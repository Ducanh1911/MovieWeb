using System.ComponentModel.DataAnnotations;

namespace MovieWebApp.Application.DTOs.Auth
{
    public class RevokeTokenRequestDto
    {
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
