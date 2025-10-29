using MovieWebApp.Application.DTOs;
using MovieWebApp.Domain.Entities;

namespace MovieWebApp.Application.Interfaces
{
    public interface IGenreService
    {
        Task<IEnumerable<Genre>> GetAllAsync();
        Task<Genre> GetByIdAsync(int id);
   
        Task<Genre> CreateGenreAsync(GenreDto genreDto);
        Task<bool> DeleteGenreAsync(int id);
    }
}
