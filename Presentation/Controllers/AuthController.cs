using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MovieWebApp.Application.DTOs;
using MovieWebApp.Application.DTOs.Auth;
using MovieWebApp.Application.Interfaces;
using System.Security.Claims;

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
    [Authorize]

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var user = await _authService.GetByIdAsync(id);
            return Ok(user);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Lỗi máy chủ", detail = ex.Message });
        }
    } 

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        _logger.LogInformation("Bắt đầu đăng ký cho email: {Email}", model.Email);

        try
        {
            var response = await _authService.RegisterAsync(model);
            _logger.LogInformation("Đăng ký thành công cho userId {UserId}", response.UserId);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        _logger.LogInformation("Bắt đầu đăng nhập cho email: {Email}", model.Email);
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var response = await _authService.LoginAsync(model);
            _logger.LogInformation("Đăng nhập thành công cho userId {UserId}", response.UserId);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Đăng nhập thất bại cho email {Email}", model.Email);
            return Unauthorized(new { message = ex.Message });
        }
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                          ?? User.FindFirst("userId")?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            return Unauthorized("Token không hợp lệ hoặc hết hạn.");

        try
        {
            var result = await _authService.ChangePasswordAsync(userId, dto);
            if (!result)
            {
                return NotFound("Người dùng không tồn tại.");
            }

            return Ok("Đổi mật khẩu thành công.");
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Mật khẩu hiện tại không đúng cho userId {UserId}", userId);
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Mật khẩu xác nhận không khớp cho userId {UserId}", userId);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi đổi mật khẩu cho userId {UserId}", userId);
            return StatusCode(500, new { message = "Có lỗi xảy ra khi đổi mật khẩu" });
        }
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request)
    {
        try
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            var response = await _authService.RefreshTokenAsync(request.RefreshToken, ipAddress);
            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Refresh token thất bại");
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi refresh token");
            return StatusCode(500, new { message = "Lỗi server", detail = ex.Message });
        }
    }

    [HttpPost("revoke")]
    [Authorize]
    public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequestDto request)
    {
        try
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            await _authService.RevokeTokenAsync(request.RefreshToken, ipAddress);
            return Ok(new { message = "Token đã được thu hồi" });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi revoke token");
            return StatusCode(500, new { message = "Lỗi server" });
        }
    }

    [HttpPost("logout-all")]
    [Authorize]
    public async Task<IActionResult> LogoutAll()
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                              ?? User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized("Token không hợp lệ");

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            await _authService.RevokeAllUserTokensAsync(userId, ipAddress);
            
            return Ok(new { message = "Đã đăng xuất khỏi tất cả thiết bị" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi logout all");
            return StatusCode(500, new { message = "Lỗi server" });
        }
    }
}
