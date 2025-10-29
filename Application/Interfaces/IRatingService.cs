using MovieWebApp.Application.DTOs;

namespace MovieWebApp.Application.Interfaces
{
    public interface IRatingService
    {
        Task<RatingDto> CreateRatingAsync(CreateRatingDto createRatingDto, int userId);
        Task<RatingDto> UpdateRatingAsync(int ratingId, UpdateRatingDto updateRatingDto, int userId);
        Task<bool> DeleteRatingAsync(int ratingId, int userId);
        Task<RatingDto> GetRatingByIdAsync(int ratingId);
        Task<List<RatingDto>> GetRatingsByMovieIdAsync(int movieId);
        Task<List<RatingDto>> GetRatingsByUserIdAsync(int userId);
        Task<RatingDto> GetUserRatingForMovieAsync(int movieId, int userId);
        Task<double> CalculateAverageRatingAsync(int movieId);
    }
}




