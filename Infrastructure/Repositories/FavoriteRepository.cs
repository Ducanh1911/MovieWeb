using MovieWebApp.Domain.Entities;
using MovieWebApp.Domain.Interfaces;
using MovieWebApp.Infrastructure.Data;
using MovieWebApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace MovieWebApp.Infrastructure.Repositories
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly ApplicationDbContext _context;
        public FavoriteRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Favorite>> GetFavoritesByUserAsync(int userId)
        {
            return await _context.favorites
                .Where(f => f.UserId == userId && f.Movie.IsDeleted == false)
                .Include(f => f.Movie)
                .ToListAsync();
        }

        public async Task<Favorite> GetFavoriteAsync(int userId, int movieId)
        {
            return await _context.favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.MovieId == movieId);
        }

        public async Task AddAsync(Favorite favorite)
        {
            await _context.favorites.AddAsync(favorite);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(Favorite favorite)
        {
            _context.favorites.Remove(favorite);
            await _context.SaveChangesAsync();

        }


    }
}
