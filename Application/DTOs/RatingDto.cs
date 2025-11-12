using System.ComponentModel.DataAnnotations;

namespace MovieWebApp.Application.DTOs
{
    public class RatingDto
    {
        public int RatingId { get; set; }
        public int UserId { get; set; }
        public int MovieId { get; set; }
        
        [Range(1, 5, ErrorMessage = "Đánh giá phải từ 1 đến 5 sao")]
        public int StarRating { get; set; }
        
        public string? Review { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        public string UserName { get; set; }
    }
    
    public class CreateRatingDto
    {
        [Required]
        public int MovieId { get; set; }
        
        [Required]
        [Range(1, 5, ErrorMessage = "Đánh giá phải từ 1 đến 5 sao")]
        public int StarRating { get; set; }
        
        public string? Review { get; set; }
    }
    
    public class UpdateRatingDto
    {
        [Range(1, 5, ErrorMessage = "Đánh giá phải từ 1 đến 5 sao")]
        public int StarRating { get; set; }
        
        public string? Review { get; set; }
    }
}




