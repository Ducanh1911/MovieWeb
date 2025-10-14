using System.ComponentModel.DataAnnotations;

namespace MovieWebApp.Application.DTOs
{
    public class CommentDto
    {
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        // Thông tin người dùng
        public string UserName { get; set; }
    }
    
    public class CreateCommentDto
    {
        [Required]
        public int MovieId { get; set; }
        
        [Required]
        [MaxLength(1000, ErrorMessage = "Bình luận không được quá 1000 ký tự")]
        public string Content { get; set; }
    }
    
    public class UpdateCommentDto
    {
        [Required]
        [MaxLength(1000, ErrorMessage = "Bình luận không được quá 1000 ký tự")]
        public string Content { get; set; }
    }
}

