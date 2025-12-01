using MovieWebApp.Domain.Entities;
using MovieWebApp.Domain.SeedWorks;

namespace MovieWebApp.Domain.Interfaces
{
    public interface IRatingRepository : IRepository<Rating, int>
    {
       
        Task<Rating> GetByIdAsync(int ratingId);
        Task<List<Rating>> GetByMovieIdAsync(int movieId);
        Task<List<Rating>> GetByUserIdAsync(int userId);
        Task<Rating> GetUserRatingForMovieAsync(int movieId, int userId);
        Task<double> GetAverageRatingAsync(int movieId);
        Task<List<Rating>> GetAllAsync();
    }
}


