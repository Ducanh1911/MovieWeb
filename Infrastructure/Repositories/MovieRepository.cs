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
