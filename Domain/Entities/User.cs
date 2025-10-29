using System.ComponentModel.DataAnnotations;

namespace MovieWebApp.Domain.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required, MaxLength(100)]
        public string UserName { get; set; }

        [Required, MaxLength(100)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; } 
        public string Role { get; set; } = "User";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Quan hệ với Favorite
        public virtual ICollection<Favorite> Favorites { get; set; }
        
        // Quan hệ với Rating và Comment
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
