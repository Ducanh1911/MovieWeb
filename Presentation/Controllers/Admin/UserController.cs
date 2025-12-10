using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieWebApp.Application.DTOs;
using MovieWebApp.Application.Interfaces;

namespace MovieWebApp.Presentation.Controllers.Admin
{
    [Route("api/admin/users")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly ILogger<UserController> _logger;

        public UserController(IAdminService adminService, ILogger<UserController> logger)
        {
            _adminService = adminService;
            _logger = logger;
        }

        /// <summary>
        /// GET: api/User
        /// Lấy danh sách tất cả người dùng
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                _logger.LogInformation("Admin getting all users");
                var users = await _adminService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all users");
                return BadRequest(new { message = "Lỗi khi lấy danh sách người dùng", error = ex.Message });
            }
        }

        /// <summary>
        /// GET: api/User/{id}
        /// Lấy thông tin chi tiết một người dùng
        /// </summary>
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            try
            {
                _logger.LogInformation("Getting user with ID: {UserId}", userId);
                var user = await _adminService.GetUserByIdAsync(userId);
                
                if (user == null)
                    return NotFound(new { message = "Không tìm thấy người dùng" });

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user {UserId}", userId);
                return BadRequest(new { message = "Lỗi khi lấy thông tin người dùng", error = ex.Message });
            }
        }

        /// <summary>
        /// PUT: api/User/{id}/role
        /// Cập nhật vai trò người dùng (User/Admin)
        /// </summary>
        [HttpPut("{userId}/role")]
        public async Task<IActionResult> UpdateUserRole(int userId, [FromBody] UpdateUserRoleDto dto)
        {
            try
            {
                if (string.IsNullOrEmpty(dto.Role))
                    return BadRequest(new { message = "Role không được để trống" });

                if (dto.Role != "User" && dto.Role != "Admin")
                    return BadRequest(new { message = "Role chỉ được là 'User' hoặc 'Admin'" });

                _logger.LogInformation("Updating role for user {UserId} to {Role}", userId, dto.Role);
                var result = await _adminService.UpdateUserRoleAsync(userId, dto.Role);
                
                if (!result)
                    return NotFound(new { message = "Không tìm thấy người dùng" });

                return Ok(new { message = "Cập nhật vai trò thành công" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating role for user {UserId}", userId);
                return BadRequest(new { message = "Lỗi khi cập nhật vai trò", error = ex.Message });
            }
        }

        /// <summary>
        /// DELETE: api/User/{id}
        /// Xóa người dùng
        /// </summary>
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            try
            {
                _logger.LogInformation("Deleting user {UserId}", userId);
                var result = await _adminService.DeleteUserAsync(userId);
                
                if (!result)
                    return NotFound(new { message = "Không tìm thấy người dùng" });

                return Ok(new { message = "Xóa người dùng thành công" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user {UserId}", userId);
                return BadRequest(new { message = "Lỗi khi xóa người dùng", error = ex.Message });
            }
        }
    }
}
