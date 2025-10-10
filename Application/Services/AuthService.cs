using Microsoft.IdentityModel.Tokens;
using MovieWebApp.Application.DTOs;
using MovieWebApp.Application.DTOs.Auth;
using MovieWebApp.Application.Interfaces;
using MovieWebApp.Domain.Entities;
using MovieWebApp.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IUserRepository userRepository, IConfiguration configuration, ILogger<AuthService> logger)
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _logger = logger;
    }


    public async Task<User> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
        {
            throw new KeyNotFoundException($"Không tìm thấy user với id = {id}");
        }

        return user;
    }
    public async Task<AuthResponseDto> RegisterAsync(RegisterDto model)
    {
        _logger.LogInformation("Đăng ký user mới với email: {Email}", model.Email);

        var existingUser = await _userRepository.GetByEmailAsync(model.Email);
        if (existingUser != null)
        {
            _logger.LogWarning("Email {Email} đã tồn tại", model.Email);
            throw new Exception("Email đã được sử dụng");
        }

        if (model.Password != model.ConfirmPassword)
        {
            _logger.LogWarning("Mật khẩu xác nhận không khớp cho email {Email}", model.Email);
            throw new Exception("Mật khẩu xác nhận không khớp");
        }

        if (model.Role != "User" && model.Role != "Admin")
        {
            _logger.LogWarning("Vai trò {Role} không hợp lệ", model.Role);
            throw new Exception("Vai trò không hợp lệ. Chỉ hỗ trợ 'User' hoặc 'Admin'.");
        }

        var user = new User
        {
            UserName = model.UserName,
            Email = model.Email,
            PasswordHash = HashPassword(model.Password),
            Role = model.Role,
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.AddAsync(user);
        _logger.LogInformation("Tạo user thành công: userId = {UserId}", user.UserId);

        var token = GenerateJwtToken(user);
        return new AuthResponseDto
        {
            Token = token,
            Expiration = DateTime.UtcNow.AddHours(1),
            UserId = user.UserId,
            Email = user.Email,
            Role = user.Role
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto model)
    {
        _logger.LogInformation("Đăng nhập với email: {Email}", model.Email);

        var user = await _userRepository.GetByEmailAsync(model.Email);
        if (user == null || !VerifyPassword(model.Password, user.PasswordHash))
        {
            _logger.LogWarning("Đăng nhập thất bại cho email: {Email}", model.Email);
            throw new Exception("Email hoặc mật khẩu không đúng");
        }

        _logger.LogInformation("Đăng nhập thành công: userId = {UserId}", user.UserId);

        var token = GenerateJwtToken(user);
        return new AuthResponseDto
        {
            Token = token,
            Expiration = DateTime.UtcNow.AddHours(1),
            UserId = user.UserId,
            Email = user.Email,
            Role = user.Role
        };
    }

    public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto dto)
    {
        _logger.LogInformation("Đổi mật khẩu cho userId {UserId}", userId);

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            _logger.LogWarning("UserId {UserId} không tồn tại", userId);
            return false;
        }

        if (!VerifyPassword(dto.CurrentPassword, user.PasswordHash))
        {
            _logger.LogWarning("Mật khẩu hiện tại không đúng cho userId {UserId}", userId);
            throw new UnauthorizedAccessException("Mật khẩu hiện tại không đúng.");
        }

        if (dto.NewPassword != dto.ConfirmPassword)
        {
            _logger.LogWarning("Mật khẩu mới và xác nhận không khớp cho userId {UserId}", userId);
            throw new InvalidOperationException("Mật khẩu xác nhận không khớp.");
        }

        user.PasswordHash = HashPassword(dto.NewPassword);
        await _userRepository.UpdateAsync(user);

        _logger.LogInformation("Đổi mật khẩu thành công cho userId {UserId}", userId);
        return true;
    }
    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    private bool VerifyPassword(string password, string storedHash)
    {
        return HashPassword(password) == storedHash;
    }

    private string GenerateJwtToken(User user)
    {
        var jwtSettings = _configuration.GetSection("Jwt");

        if (string.IsNullOrEmpty(jwtSettings["Key"]))
        {
            _logger.LogError("Jwt:Key trong appsettings.json bị null hoặc rỗng!");
            throw new ArgumentNullException("Jwt:Key", "Khóa JWT không được null.");
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role ?? "User"),
            new Claim("userId", user.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
