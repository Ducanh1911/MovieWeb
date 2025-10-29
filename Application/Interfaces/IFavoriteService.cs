using MovieWebApp.Application.DTOs;
using MovieWebApp.Domain.Entities;

namespace MovieWebApp.Application.Interfaces
{
    public interface IFavoriteService
    {
        Task<IEnumerable<Movie>> GetFavoritesByUserAsync(int userId);
        Task<Favorite> AddFavoriteAsync(FavoriteDto dto);
        Task<bool> RemoveFavoriteAsync(int userId, int movieId);


    }
}
