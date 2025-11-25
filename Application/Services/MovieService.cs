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
        private readonly CloudinaryService _cloudinaryService;

        public MovieService(IMovieRepository movieRepository, IGenreRepository genreRepository, CloudinaryService cloudinaryService)
        {
            _movieRepository = movieRepository;
            _genreRepository = genreRepository;
            _cloudinaryService = cloudinaryService;
        }
        public async Task<bool> DeleteMovieAsync(int id)
        {
            var movie = await _movieRepository.GetByIdAsync(id);
            if (movie == null)
            {
                return false;
            }
            movie.IsDeleted = !movie.IsDeleted; // Toggle IsDeleted
            await _movieRepository.UpdateAsync(movie);
            return true;
        }

        public async Task<IEnumerable<Movie>> GetAllMoviesClientAsync()
        {

            var movies = await _movieRepository.GetMovieClientAsync();

            return movies;
        }
        public async Task<IEnumerable<Movie>> GetAllMoviesAdminAsync()
        {

            var movies = await _movieRepository.GetMovieAdminAsync();

            return movies;
        }

        public async Task<(IEnumerable<Movie> Movies, int TotalCount)> GetPagedMoviesAsync(int pageNumber, int pageSize)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            return await _movieRepository.GetPagedAsync(pageNumber, pageSize);
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

        public async Task<Movie> CreateMovieWithFilesAsync(MovieUploadRequest request)
        {
            try
            {
                string posterUrl = null;
                string videoUrl = null;

                if (request.PosterFile != null)
                {
                    posterUrl = await _cloudinaryService.UploadImageAsync(request.PosterFile, "movies/posters");
                }

                if (request.VideoFile != null)
                {
                    videoUrl = await _cloudinaryService.UploadVideoAsync(request.VideoFile, "movies/videos");
                }

                var movie = new Movie
                {
                    MovieName = request.MovieName,
                    Description = request.Description,
                    ReleaseYear = request.ReleaseYear,
                    Country = request.Country,
                    Language = request.Language,
                    Poster = posterUrl,
                    VideoUrl = videoUrl,
                    Genres = new List<Genre>(),
                    createdAt = DateTime.UtcNow
                };

                if (request.GenreIds != null && request.GenreIds.Any())
                {
                    var genres = await _genreRepository.GetGenresByIdsAsync(request.GenreIds);
                    if (!genres.Any())
                    {
                        throw new ArgumentException("Không tìm thấy thể loại nào với các ID được cung cấp.");
                    }
                    foreach (var genre in genres)
                    {
                        movie.Genres.Add(genre);
                    }
                }
                else
                {
                    throw new ArgumentException("Phải chọn ít nhất 1 thể loại cho phim.");
                }

                var created = await _movieRepository.AddAsync(movie);
                return created;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tạo phim: {ex.Message}", ex);
            }
        }

        public async Task<Movie?> UpdateMovieWithFilesAsync(int id, MovieUploadRequest request)
        {
            var movie = await _movieRepository.GetByIdAsync(id);
            if (movie == null)
            {
                return null;
            }

            try
            {
                if (request.PosterFile != null)
                {
                    movie.Poster = await _cloudinaryService.UploadImageAsync(request.PosterFile, "movies/posters");
                }

                if (request.VideoFile != null)
                {
                    movie.VideoUrl = await _cloudinaryService.UploadVideoAsync(request.VideoFile, "movies/videos");
                }

                movie.MovieName = request.MovieName;
                movie.Description = request.Description;
                movie.ReleaseYear = request.ReleaseYear;
                movie.Country = request.Country;
                movie.Language = request.Language;

                if (request.GenreIds != null && request.GenreIds.Any())
                {
                    var genres = await _genreRepository.GetGenresByIdsAsync(request.GenreIds);
                    if (!genres.Any())
                    {
                        throw new ArgumentException("Không tìm thấy thể loại nào với các ID được cung cấp.");
                    }
                    movie.Genres.Clear();
                    foreach (var genre in genres)
                    {
                        movie.Genres.Add(genre);
                    }
                }

                var updated = await _movieRepository.UpdateAsync(movie);
                return updated;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật phim: {ex.Message}", ex);
            }
        }

    }
}
