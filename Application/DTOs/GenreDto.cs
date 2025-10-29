using System.ComponentModel.DataAnnotations;

namespace MovieWebApp.Application.DTOs
{
    public class GenreDto : ICloneable
    {
        [Required(ErrorMessage = "Tên thể loại là bắt buộc")]
        [MaxLength(50, ErrorMessage = "Tên thể loại tối đa 50 ký tự")]
        public string Name { get; set; }

        public object Clone()
        {
            return new GenreDto
            {
                Name = this.Name
            };
        }

        public class Builder
        {
            private readonly GenreDto _genre = new GenreDto();

            public Builder SetName(string name)
            {
                _genre.Name = name;
                return this;
            }

            public GenreDto Build()
            {
                return _genre;
            }
        }
    }
}
