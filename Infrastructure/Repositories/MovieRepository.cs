using Microsoft.EntityFrameworkCore;
using MovieWebApp.Domain.Entities;
using MovieWebApp.Domain.Interfaces;
using MovieWebApp.Domain.SeedWorks;
using MovieWebApp.Infrastructure.Data;
using MovieWebApp.Infrastructure.SeedWorks;

namespace MovieWebApp.Infrastructure.Repositories
{
    public class MovieRepository : RepositoryBase<Movie, int>, IMovieRepository
    {
        private readonly ApplicationDbContext _context;
        public MovieRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Movie>> GetMovieClientAsync()
        {
            return await _context.movies
                   .Include(m => m.Genres)
                   .Where(m => m.IsDeleted == false)
                   .ToListAsync();
        }
        public async Task<IEnumerable<Movie>> GetMovieAdminAsync()
        {
            return await _context.movies
                   .Include(m => m.Genres)
                   .ToListAsync();
        }

        public async Task<Movie> GetByIdAsync(int id)
        {
            return await _context.movies
                .Include(m => m.Genres)
                .FirstOrDefaultAsync(m => m.MovieId == id);
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

        public async Task<(IEnumerable<Movie> Movies, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, string search = "", string genre = "")
        {
            var query = _context.movies
                .Include(m => m.Genres)
                .Where(m => m.IsDeleted == false);

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(m => m.MovieName.Contains(search));
            }

            // Apply genre filter
            if (!string.IsNullOrWhiteSpace(genre))
            {
                query = query.Where(m => m.Genres.Any(g => g.Name == genre));
            }

            var totalCount = await query.CountAsync();

            var movies = await query
                .OrderByDescending(m => m.createdAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (movies, totalCount);
        }
    }
}