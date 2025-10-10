using MovieWebApp.Application.DTOs;
using MovieWebApp.Application.Interfaces;
using MovieWebApp.Domain.Entities;
using MovieWebApp.Domain.Interfaces;

namespace MovieWebApp.Application.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        public MovieService(IMovieRepository movieRepository)
        {
           _movieRepository = movieRepository;
        }
        public async Task<Movie> CreateMovieAsync(MovieDto dto)
        {
            var movie = new Movie
            {
                MovieName = dto.MovieName,
                Description = dto.Description,
                ReleaseYear = dto.ReleaseYear,
                Country = dto.Country,
                Language = dto.Language,
                Poster = dto.Poster,
                VideoUrl = dto.VideoUrl,
                createdAt = dto.createdAt ?? DateTime.Now
            };

            return await _movieRepository.AddAsync(movie, dto.GenreIds);
        } 

        public async Task<bool> DeleteMovieAsync(int id)
        {
            return await _movieRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
        {   
            return await _movieRepository.GetAsync();
            
        }

        public async Task<Movie?> GetMovieByIdAsync(int id)
        {
            return await _movieRepository.GetByIdAsync(id);
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
            var movie = new Movie
            {
                MovieId = id,
                MovieName = dto.MovieName,
                Description = dto.Description,
                ReleaseYear = dto.ReleaseYear,
                Country = dto.Country,
                Language = dto.Language,
                Poster = dto.Poster,
                VideoUrl = dto.VideoUrl,
                createdAt = dto.createdAt ?? DateTime.Now
            };

            return await _movieRepository.UpdateAsync(movie, dto.GenreIds);
        }

        public async Task<(IEnumerable<Movie> Movies, int TotalCount)> GetPagedMoviesAsync(int pageNumber, int pageSize)
        {
            return await _movieRepository.GetPagedAsync(pageNumber, pageSize);
        }

        //public async Task<bool> UpdatePosterAsync(int movieId, string posterPath)

        //{
        //    var movie = await _movieRepository.GetByIdAsync(movieId);
        //    if (movie == null) return false;

        //    movie.Poster = posterPath;
        //    await _movieRepository.UpdateAsync(movie, movie.Genres.Select(g => g.GenresId).ToList());

        //    return true;
        //}

        //public async Task<bool> UpdateVideoUrlAsync(int movieId, string videoPath)
        //{
        //    var movie = await _movieRepository.GetByIdAsync(movieId);
        //    if (movie == null) return false;

        //    movie.VideoUrl = videoPath;
        //    await _movieRepository.UpdateAsync(movie, movie.Genres.Select(g => g.GenresId).ToList());

        //    return true;
        //}

    }
}
