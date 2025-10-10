using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MovieWebApp.Domain.Entities
{
    public class Genre
    {
        [Key]
        public int GenresId { get; set; }

        public string Name { get; set; }
        [JsonIgnore]
        public virtual ICollection<Movie> Movies { get; set; }
    }
}
