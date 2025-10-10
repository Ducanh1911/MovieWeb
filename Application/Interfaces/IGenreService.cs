using MovieWebApp.Application.DTOs;
using MovieWebApp.Domain.Entities;

namespace MovieWebApp.Application.Interfaces
{
    public interface IGenreService
    {
        Task<IEnumerable<Genre>> GetAllAsync();
        Task<Genre> GetByIdAsync(int id);
        Task<Genre> CreateGenreAsync(GenreDto genredto);
        //Task<Genre> UpdateGenreAsync(int id, GenreDto genre);
        Task<bool> DeleteGenreAsync(int id);
    }
}
