using MovieWebApp.Domain.Entities;

namespace MovieWebApp.Domain.Interfaces
{
    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetAsync();
        Task<Movie> GetByIdAsync(int id);
        Task<Movie> AddAsync(Movie movie, List<int> genreIds);
        Task<Movie> UpdateAsync(Movie movie, List<int> genreIds);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Movie>> SearchByNameAsync(string keyword);
        Task<(IEnumerable<Movie> Movies, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize);

    }
}
