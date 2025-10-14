using System.ComponentModel.DataAnnotations;

namespace MovieWebApp.Application.DTOs.Auth
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        // Chấp nhận chữ, số, @, ., _, -
        [RegularExpression(@"^[a-zA-Z0-9@._\-]+$", ErrorMessage = "Email chứa ký tự không hợp lệ")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        // Không cho phép khoảng trắng và các ký tự nguy hiểm như ', ", <, >, ;
        [RegularExpression(@"^[^\s'""<>;]+$", ErrorMessage = "Mật khẩu chứa ký tự không hợp lệ")]
        public string Password { get; set; } = string.Empty;
    }
}
