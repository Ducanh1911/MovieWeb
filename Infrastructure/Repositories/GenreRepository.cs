using Microsoft.EntityFrameworkCore;
using MovieWebApp.Application.DTOs;
using MovieWebApp.Domain.Entities;
using MovieWebApp.Domain.Interfaces;
using MovieWebApp.Infrastructure.Data;

namespace MovieWebApp.Infrastructure.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly ApplicationDbContext _context;
        public GenreRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Genre>> GetGenresByIdsAsync(IEnumerable<int> genreIds)
        {
            return await _context.genres
                .Where(g => genreIds.Contains(g.GenresId))
                .ToListAsync();
        }
        public async Task<IEnumerable<Genre>> getAsync()
        {
            var genre = await _context.genres.ToListAsync();
            return genre;
        }

        public async Task<Genre> getByIdAsync(int id)
        {
            var genre = await _context.genres.FirstOrDefaultAsync(g=>g.GenresId == id);
            return genre;
        }
        public async Task<Genre> CreateAsync(Genre genre)
        {
            await _context.genres.AddAsync(genre);
            await _context.SaveChangesAsync();
            return genre;

        }

        public async Task<Genre> updateAsync(Genre genre)
        {
            _context.Update(genre);
            await _context.SaveChangesAsync();
            return genre;
        }

        public async Task<bool> deleteAsync(int id)
        {
            var genre = await _context.genres
                .Include(g => g.Movies)
                .FirstOrDefaultAsync(g => g.GenresId == id);
            
            _context.genres.Remove(genre);
            await _context.SaveChangesAsync();
            return true;
        }       
    }
}
