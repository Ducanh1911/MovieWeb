using System.ComponentModel.DataAnnotations;

namespace MovieWebApp.Application.DTOs
{
    public class GenreDto
    {
        [Required(ErrorMessage = "Tên phim là bắt buộc")]
        [MaxLength(50, ErrorMessage = "Tên thể loại tối đa 50 ký tự")]
        public string Name { get; set; }
    }
}
