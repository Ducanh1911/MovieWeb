using MovieWebApp.Domain.Entities;

namespace MovieWebApp.Domain.Interfaces
{
    public interface IRatingRepository
    {
        Task<Rating> CreateAsync(Rating rating);
        Task<Rating> UpdateAsync(Rating rating);
        Task<bool> DeleteAsync(int ratingId);
        Task<Rating> GetByIdAsync(int ratingId);
        Task<List<Rating>> GetByMovieIdAsync(int movieId);
        Task<List<Rating>> GetByUserIdAsync(int userId);
        Task<Rating> GetUserRatingForMovieAsync(int movieId, int userId);
        Task<double> GetAverageRatingAsync(int movieId);
        Task<List<Rating>> GetAllAsync();
    }
}


