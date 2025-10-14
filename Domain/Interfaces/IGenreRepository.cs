using MovieWebApp.Domain.Entities;

namespace MovieWebApp.Domain.Interfaces
{
    public interface IGenreRepository
    {
        Task<IEnumerable<Genre>> getAsync();
        Task<Genre> getByIdAsync(int id);
        Task<Genre> CreateAsync(Genre genre);
        Task<Genre> updateAsync(Genre genre);
        Task<bool> deleteAsync(int id);

    }
}
