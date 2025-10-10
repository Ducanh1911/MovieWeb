using Microsoft.EntityFrameworkCore;
using MovieWebApp.Domain.Entities;
using MovieWebApp.Domain.Interfaces;
using MovieWebApp.Infrastructure.Data;

namespace MovieWebApp.Infrastructure.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly ApplicationDbContext _context;
        public MovieRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        // phân trang 
        public async Task<(IEnumerable<Movie> Movies, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _context.movies
                    .Include(m => m.Genres)
                    .AsQueryable();

            var totalCount = await query.CountAsync();

            var movies = await query
                    .OrderBy(m => m.MovieId)   
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

            return (movies, totalCount);
        }   

        public async Task<IEnumerable<Movie>> GetAsync()
        {
            return await _context.movies
                   .Include(m => m.Genres)
                   .Where(m => m.IsDeleted == false)
                   .ToListAsync();
        }

        public async Task<Movie> GetByIdAsync(int id)
        {
            return await _context.movies
                .Include(m => m.Genres)
                .FirstOrDefaultAsync(m => m.MovieId == id);
        }

        public async Task<Movie> AddAsync(Movie movie, List<int> genreIds)
        {
            var genres = await _context.genres
                .Where(g => genreIds.Contains(g.GenresId))
                .ToListAsync();

            var notFoundGenreIds = genreIds.Except(genres.Select(g => g.GenresId)).ToList();
            if (notFoundGenreIds.Any())
            {
                throw new InvalidOperationException(
                    $"Các genreId sau không tồn tại: {string.Join(", ", notFoundGenreIds)}"
                );
            }

            // Gán genres cho movie
            movie.Genres = genres;

            // Thêm vào DB
            await _context.movies.AddAsync(movie);
            await _context.SaveChangesAsync();

            return movie;
        }

       
        public async Task<Movie> UpdateAsync(Movie movie, List<int> genreIds)
        {
            var existingMovie = await _context.movies
                .Include(m => m.Genres)
                .FirstOrDefaultAsync(m => m.MovieId == movie.MovieId);


            if (existingMovie == null) return null;

            // cập nhật property
            existingMovie.MovieName = movie.MovieName;
            existingMovie.Description = movie.Description;
            existingMovie.ReleaseYear = movie.ReleaseYear;
            existingMovie.Country = movie.Country;
            existingMovie.Language = movie.Language;
            existingMovie.Poster = movie.Poster;
            existingMovie.VideoUrl = movie.VideoUrl;
            existingMovie.createdAt = movie.createdAt;

            if (genreIds == null || !genreIds.Any())
            {
                throw new InvalidOperationException("Phải chọn ít nhất 1 thể loại (genreId).");
            }

            // cập nhật genres
            var genres = await _context.genres
                .Where(g => genreIds.Contains(g.GenresId))
                .ToListAsync();

            var notFoundGenreIds = genreIds.Except(genres.Select(g => g.GenresId)).ToList();
            if (notFoundGenreIds.Any())
            {
                throw new InvalidOperationException(
                    $"Các genreId sau không tồn tại: {string.Join(", ", notFoundGenreIds)}"
                );
            }
            existingMovie.Genres = genres;

            _context.movies.Update(existingMovie);
            await _context.SaveChangesAsync();

            return existingMovie;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var movie = await _context.movies.FindAsync(id);
            if (movie == null) return false;
            //if (movie.IsDeleted == true) return false;
            // xoa mem
            movie.IsDeleted = true;
            _context.movies.Update(movie);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Movie>> SearchByNameAsync(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
                return new List<Movie>();

            return await _context.movies
                .Include(m => m.Genres)
                .Where(m => m.MovieName.Contains(keyword))
                .ToListAsync();
        }
    }
}
