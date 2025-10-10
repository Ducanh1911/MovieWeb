using System.ComponentModel.DataAnnotations;

namespace MovieWebApp.Application.DTOs
{
    public class MovieDto
    {
        [Required(ErrorMessage = "Tên phim là bắt buộc")]
        [MaxLength(100, ErrorMessage = "Tên phim tối đa 100 ký tự")]
        public string MovieName { get; set; }
        public string Description { get; set; }
        [Range(2000, 2025, ErrorMessage = "Năm phát hành phải từ 2000 đến 2025")]
        public int ReleaseYear { get; set; }
        public string Country { get; set; }
        public string Language { get; set; }
        public string Poster { get; set; }
        public String VideoUrl { get; set; }
        public Nullable<System.DateTime> createdAt { get; set; } = default(Nullable<System.DateTime>);
        public List<int> GenreIds { get; set; } = new List<int>();
    }
}
