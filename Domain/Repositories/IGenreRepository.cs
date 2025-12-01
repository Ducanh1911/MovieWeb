using MovieWebApp.Domain.Entities;
using MovieWebApp.Domain.SeedWorks;

namespace MovieWebApp.Domain.Interfaces
{
    public interface IGenreRepository : IRepository<Genre, int>
    {
       
        Task<IEnumerable<Genre>> GetGenresByIdsAsync(IEnumerable<int> genreIds);

    }
}
