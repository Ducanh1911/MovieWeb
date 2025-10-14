using System.ComponentModel.DataAnnotations;

namespace MovieWebApp.Domain.Entities
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [Required]
        public int MovieId { get; set; }
        
        [Required]
        [MaxLength(1000, ErrorMessage = "Bình luận không được quá 1000 ký tự")]
        public string Content { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual User User { get; set; }
        public virtual Movie Movie { get; set; }
    }
}

