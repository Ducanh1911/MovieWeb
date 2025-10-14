using System.ComponentModel.DataAnnotations;

namespace MovieWebApp.Domain.Entities
{
    public class Rating
    {
        [Key]
        public int RatingId { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [Required]
        public int MovieId { get; set; }
        
        [Range(1, 5, ErrorMessage = "Đánh giá phải từ 1 đến 5 sao")]
        public int StarRating { get; set; }
        
        public string? Review { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual User User { get; set; }
        public virtual Movie Movie { get; set; }
    }
}

