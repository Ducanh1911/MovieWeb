using MovieWebApp.Domain.Entities;
using MovieWebApp.Domain.SeedWorks;

namespace MovieWebApp.Domain.Interfaces
{
    public interface IGenreRepository : IRepository<Genre, int>
    {
        //Task<IEnumerable<Genre>> getAsync();
        //Task<Genre> getByIdAsync(int id);
        //Task<Genre> CreateAsync(Genre genre);
        //Task<Genre> updateAsync(Genre genre);
        //Task<bool> deleteAsync(int id);
        Task<IEnumerable<Genre>> GetGenresByIdsAsync(IEnumerable<int> genreIds);

    }
}
