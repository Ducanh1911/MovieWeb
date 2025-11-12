using MovieWebApp.Domain.Entities;
using MovieWebApp.Domain.SeedWorks;

namespace MovieWebApp.Domain.Interfaces
{
    public interface IFavoriteRepository : IRepository<Favorite, int>
    {
        Task<IEnumerable<Favorite>> GetFavoritesByUserAsync(int userId);
        Task<Favorite> GetFavoriteAsync(int userId, int movieId);
        //Task AddAsync(Favorite favorite);
        Task RemoveAsync(Favorite favorite);
        Task<List<Favorite>> GetAllAsync();

    }
}
