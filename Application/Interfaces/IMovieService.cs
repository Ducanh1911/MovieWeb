using MovieWebApp.Application.DTOs;
using MovieWebApp.Domain.Entities;

namespace MovieWebApp.Application.Interfaces
{
    public interface IMovieService
    {
        Task<Movie> CreateMovieAsync(MovieDto movieDto);
        Task<IEnumerable<Movie>> GetAllMoviesAsync();
        Task<Movie?> GetMovieByIdAsync(int id);
        Task<Movie?> UpdateMovieAsync(int id, MovieDto dto);
        Task<bool> DeleteMovieAsync(int id);
        Task<IEnumerable<MovieDto>> SearchMoviesAsync(string keyword);
        //Task<(IEnumerable<Movie> Movies, int TotalCount)> GetPagedMoviesAsync(int pageNumber, int pageSize);
        Task<bool> IncrementViewCountAsync(int movieId);
    }
}
