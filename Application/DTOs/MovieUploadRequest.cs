using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace MovieWebApp.Application.DTOs
{
    public class MovieCreateRequest
    {
        [Required(ErrorMessage = "Tên phim là bắt buộc")]
        [MaxLength(100, ErrorMessage = "Tên phim tối đa 100 ký tự")]
        public string MovieName { get; set; }

        public string? Description { get; set; }
        [Range(1975, 2025, ErrorMessage = "Năm phát hành phải từ 1975 đến 2025")]
        public int ReleaseYear { get; set; }

        public string? Country { get; set; }

        public string? Language { get; set; }

        [Required(ErrorMessage = "Phải chọn ít nhất 1 thể loại")]
        public List<int> GenreIds { get; set; } = new List<int>();

        // Files upload
        [Required(ErrorMessage = "Vui lòng thêm poster")]

        public IFormFile PosterFile { get; set; }
        [Required(ErrorMessage = "Vui lòng thêm video")]

        public IFormFile VideoFile { get; set; }
    }
    public class MovieUpdateRequest
    {
        [Required(ErrorMessage = "Tên phim là bắt buộc")]
        [MaxLength(100, ErrorMessage = "Tên phim tối đa 100 ký tự")]
        public string MovieName { get; set; }

        public string? Description { get; set; }
        
        [Range(1975, 2025, ErrorMessage = "Năm phát hành phải từ 1975 đến 2025")]
        public int ReleaseYear { get; set; }

        public string? Country { get; set; }

        public string? Language { get; set; }

        [MinLength(1, ErrorMessage = "Phải chọn ít nhất 1 thể loại")]
        public List<int> GenreIds { get; set; } = new();

        public IFormFile? PosterFile { get; set; }

        public IFormFile? VideoFile { get; set; }
    }
}
