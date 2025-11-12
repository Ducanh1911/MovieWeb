using MovieWebApp.Application.DTOs;
using MovieWebApp.Application.Interfaces;
using MovieWebApp.Domain.Entities;
using MovieWebApp.Domain.Interfaces;
using MovieWebApp.Infrastructure.SeedWorks;
using MovieWebApp.Infrastructure.Repositories;


namespace MovieWebApp.Application.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IGenreRepository _genreRepository;

        public MovieService(IMovieRepository movieRepository, IGenreRepository genreRepository)
        {
            _movieRepository = movieRepository;
            _genreRepository = genreRepository;
        }
        public async Task<Movie> CreateMovieAsync(MovieDto dto)
        {
            try
            {
                var movie = new Movie
                {
                    MovieName = dto.MovieName,
                    Description = dto.Description,
                    ReleaseYear = dto.ReleaseYear,
                    Country = dto.Country,
                    Language = dto.Language,
                    Genres = new List<Genre>(),
                    Poster = dto.Poster,
                    VideoUrl = dto.VideoUrl,
                };
                if (dto.GenreIds != null && dto.GenreIds.Any())
                {
                    var genres = await _genreRepository.GetGenresByIdsAsync(dto.GenreIds);
                    if (!genres.Any())
                    {
                        throw new ArgumentException("Không tìm thấy thể loại nào với các ID được cung cấp.");
                    }
                    foreach (var genre in genres)
                    {
                        movie.Genres.Add(genre);
                    }
                }
                var created = await _movieRepository.AddAsync(movie);
                return created;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }

        } 

        public async Task<bool> DeleteMovieAsync(int id)
        {
            var movie = await _movieRepository.GetByIdAsync(id);
            if (movie == null)
            {
                return false;
            }
            movie.IsDeleted = true;
            await _movieRepository.UpdateAsync(movie);
            return true;
        }

        public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
        {   

            var movies = await _movieRepository.GetAsync();
    
            return movies;
        }

        public async Task<Movie?> GetMovieByIdAsync(int id)
        {
            var movie = await _movieRepository.GetByIdAsync(id);
            return movie;
        }

        public async Task<IEnumerable<MovieDto>> SearchMoviesAsync(string keyword)
        {
            var movies = await _movieRepository.SearchByNameAsync(keyword);

            return movies.Select(m => new MovieDto
            {
                MovieName = m.MovieName,
                Description = m.Description,
                ReleaseYear = m.ReleaseYear,
                Country = m.Country,
                Language = m.Language,
                Poster = m.Poster,
                VideoUrl = m.VideoUrl,
                GenreIds = m.Genres.Select(g => g.GenresId).ToList()
            });
        }

        public async Task<Movie?> UpdateMovieAsync(int id, MovieDto dto)
        {
            var movie = await _movieRepository.GetByIdAsync(id);
            if (movie == null)
            {
                return null;
            }
            movie.MovieName = dto.MovieName;
            movie.Description = dto.Description;
            movie.ReleaseYear = dto.ReleaseYear;
            movie.Country = dto.Country;
            movie.Language = dto.Language;
            movie.Poster = dto.Poster;
            movie.VideoUrl = dto.VideoUrl;
            if (dto.GenreIds != null && dto.GenreIds.Any())
            {
                var genres = await _genreRepository.GetGenresByIdsAsync(dto.GenreIds);
                if (!genres.Any())
                {
                    throw new ArgumentException("Không tìm thấy thể loại nào với các ID được cung cấp.");
                }
                foreach (var genre in genres)
                {
                    movie.Genres.Add(genre); 
                }
            }
            var updated = await _movieRepository.UpdateAsync(movie);
            return updated;
        }

      
        public Task<bool> IncrementViewCountAsync(int movieId)
        {
            throw new NotImplementedException();
        }

    }
}
