using Microsoft.EntityFrameworkCore;
using MovieWebApp.Application.DTOs;
using MovieWebApp.Domain.Entities;
using MovieWebApp.Domain.Interfaces;
using MovieWebApp.Infrastructure.Data;
using MovieWebApp.Infrastructure.SeedWorks;

namespace MovieWebApp.Infrastructure.Repositories
{
    public class GenreRepository : RepositoryBase<Genre, int>, IGenreRepository
    {
        private readonly ApplicationDbContext _context;
        public GenreRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Genre>> GetGenresByIdsAsync(IEnumerable<int> genreIds)
        {
            return await _context.genres
                .Where(g => genreIds.Contains(g.GenresId))
                .ToListAsync();
        }
    }
}
