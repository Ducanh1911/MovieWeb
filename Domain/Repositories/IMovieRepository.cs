using MovieWebApp.Domain.Entities;
using MovieWebApp.Domain.SeedWorks;

namespace MovieWebApp.Domain.Interfaces
{
    public interface IMovieRepository : IRepository<Movie, int>
    {
        Task<IEnumerable<Movie>> GetAsync();
        Task<Movie> GetByIdAsync(int id);
        Task<IEnumerable<Movie>> SearchByNameAsync(string keyword);
        //Task<(IEnumerable<Movie> Movies, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize);

    }
}
