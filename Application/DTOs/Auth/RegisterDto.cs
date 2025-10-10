using System.ComponentModel.DataAnnotations;

namespace MovieWebApp.Application.DTOs.Auth
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Tên người dùng là bắt buộc")]
        [MaxLength(100, ErrorMessage = "Tên người dùng tối đa 100 ký tự")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [MaxLength(100, ErrorMessage = "Email tối đa 100 ký tự")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải từ 6 ký tự trở lên")]
        public string Password { get; set; } = string.Empty;

        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        public string ConfirmPassword { get; set; } = string.Empty;
        [MaxLength(50, ErrorMessage = "Vai trò tối đa 50 ký tự")]
        public string Role { get; set; } = "User";
    }
}
