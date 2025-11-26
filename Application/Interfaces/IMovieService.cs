using MovieWebApp.Application.DTOs;
using MovieWebApp.Domain.Entities;

namespace MovieWebApp.Application.Interfaces
{
    public interface IMovieService
    {
        Task<Movie> CreateMovieWithFilesAsync(MovieCreateRequest request);
        Task<Movie?> UpdateMovieWithFilesAsync(int id, MovieUpdateRequest request);
        Task<bool> DeleteMovieAsync(int id);
        Task<IEnumerable<Movie>> GetAllMoviesClientAsync();
        Task<IEnumerable<Movie>> GetAllMoviesAdminAsync();
        Task<(IEnumerable<Movie> Movies, int TotalCount)> GetPagedMoviesAsync(int pageNumber, int pageSize);
        Task<Movie?> GetMovieByIdAsync(int id);
        Task<IEnumerable<MovieDto>> SearchMoviesAsync(string keyword);
    }
}
