using MovieWebApp.Application.DTOs;
using MovieWebApp.Application.DTOs.Auth;
using MovieWebApp.Domain.Entities;

namespace MovieWebApp.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto model);
        Task<AuthResponseDto> LoginAsync(LoginDto model);
        Task<bool> ChangePasswordAsync(int Userid,ChangePasswordDto dto);
        Task<User> GetByIdAsync(int id);
        Task<AuthResponseDto> RefreshTokenAsync(string token, string? ipAddress = null);
        Task RevokeTokenAsync(string token, string? ipAddress = null);
        Task RevokeAllUserTokensAsync(int userId, string? ipAddress = null);
    }
   
}
