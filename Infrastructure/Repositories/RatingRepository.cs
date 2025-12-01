using Microsoft.EntityFrameworkCore;
using MovieWebApp.Domain.Entities;
using MovieWebApp.Domain.Interfaces;
using MovieWebApp.Infrastructure.Data;
using MovieWebApp.Infrastructure.SeedWorks;

namespace MovieWebApp.Infrastructure.Repositories
{
    public class RatingRepository : RepositoryBase<Rating, int> ,IRatingRepository
    {
        private readonly ApplicationDbContext _context;

        public RatingRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        

        public async Task<Rating> GetByIdAsync(int ratingId)
        {
            return await _context.Ratings
                .Include(r => r.User)
                .Include(r => r.Movie)
                .FirstOrDefaultAsync(r => r.RatingId == ratingId);
        }

        public async Task<List<Rating>> GetByMovieIdAsync(int movieId)
        {
            return await _context.Ratings
                .Include(r => r.User)
                .Include(r => r.Movie)
                .Where(r => r.MovieId == movieId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Rating>> GetByUserIdAsync(int userId)
        {
            return await _context.Ratings
                .Include(r => r.User)
                .Include(r => r.Movie)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<Rating> GetUserRatingForMovieAsync(int movieId, int userId)
        {
            return await _context.Ratings
                .Include(r => r.User)
                .Include(r => r.Movie)
                .FirstOrDefaultAsync(r => r.MovieId == movieId && r.UserId == userId);
        }

        public async Task<double> GetAverageRatingAsync(int movieId)
        {
            var ratings = await _context.Ratings
                .Where(r => r.MovieId == movieId)
                .Select(r => r.StarRating)
                .ToListAsync();

            return ratings.Any() ? ratings.Average() : 0;
        }

        public async Task<List<Rating>> GetAllAsync()
        {
            return await _context.Ratings
                .Include(r => r.User)
                .Include(r => r.Movie)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }
    }
}















