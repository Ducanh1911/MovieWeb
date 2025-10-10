using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieWebApp.Domain.Entities
{
    public class Favorite
    {
        [Key]
        public int FavoriteId { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        [ForeignKey("Movie")]
        public int MovieId { get; set; }

        // Navigation Property
        public virtual User User { get; set; }
        public virtual Movie Movie { get; set; }
    }
}
