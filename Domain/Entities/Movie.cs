using System.ComponentModel.DataAnnotations;

namespace MovieWebApp.Domain.Entities
{
    public class Movie
    {
        [Key]
        public int MovieId { get; set; }
        public string MovieName { get; set; }
        public string Description { get; set; }
        public int ReleaseYear { get; set; }
        public string Country { get; set; }
        public string Language { get; set; }
        public string Poster { get; set; }
        public String VideoUrl { get; set; }
  
        public double Rating { get; set; } = 0;
        public int ViewCount { get; set; } = 0;
        public Nullable<System.DateTime> createdAt { get; set; } = default(Nullable<System.DateTime>);
        public virtual ICollection<Genre> Genres { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; } // Đánh giá của người dùng
        public virtual ICollection<Comment> Comments { get; set; } // Bình luận của người dùng
        public bool IsDeleted { get; set; } = false;
    }
}