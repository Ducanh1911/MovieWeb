using MovieWebApp.Application.DTOs;
using MovieWebApp.Application.Interfaces;
using MovieWebApp.Domain.Entities;
using MovieWebApp.Domain.Interfaces;

namespace MovieWebApp.Application.Services
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IMovieRepository _movieRepository;

        public RatingService(IRatingRepository ratingRepository, IMovieRepository movieRepository)
        {
            _ratingRepository = ratingRepository;
            _movieRepository = movieRepository;
        }

        public async Task<RatingDto> CreateRatingAsync(CreateRatingDto createRatingDto, int userId)
        {
            // Kiểm tra xem người dùng đã đánh giá phim này chưa
            var existingRating = await _ratingRepository.GetUserRatingForMovieAsync(createRatingDto.MovieId, userId);
            if (existingRating != null)
            {
                throw new InvalidOperationException("Bạn đã đánh giá phim này rồi. Hãy cập nhật đánh giá thay vì tạo mới.");
            }

            var rating = new Rating
            {
                UserId = userId,
                MovieId = createRatingDto.MovieId,
                StarRating = createRatingDto.StarRating,
                Review = createRatingDto.Review,
                CreatedAt = DateTime.UtcNow
            };

            var createdRating = await _ratingRepository.CreateAsync(rating);
            
            // Cập nhật đánh giá trung bình của phim
            await UpdateMovieAverageRating(createRatingDto.MovieId);

            return MapToDto(createdRating);
        }

        public async Task<RatingDto> UpdateRatingAsync(int ratingId, UpdateRatingDto updateRatingDto, int userId)
        {
            var rating = await _ratingRepository.GetByIdAsync(ratingId);
            if (rating == null)
                throw new ArgumentException("Không tìm thấy đánh giá");

            if (rating.UserId != userId)
                throw new UnauthorizedAccessException("Bạn không có quyền cập nhật đánh giá này");

            rating.StarRating = updateRatingDto.StarRating;
            rating.Review = updateRatingDto.Review;
            rating.UpdatedAt = DateTime.UtcNow;

            var updatedRating = await _ratingRepository.UpdateAsync(rating);
            
            // Cập nhật đánh giá trung bình của phim
            await UpdateMovieAverageRating(rating.MovieId);

            return MapToDto(updatedRating);
        }

        public async Task<bool> DeleteRatingAsync(int ratingId, int userId)
        {
            var rating = await _ratingRepository.GetByIdAsync(ratingId);
            if (rating == null)
                return false;

            if (rating.UserId != userId)
                throw new UnauthorizedAccessException("Bạn không có quyền xóa đánh giá này");

            var movieId = rating.MovieId;
            var result = await _ratingRepository.DeleteAsync(ratingId);
            
            // Cập nhật đánh giá trung bình của phim
            if (result)
                await UpdateMovieAverageRating(movieId);

            return result;
        }

        public async Task<RatingDto> GetRatingByIdAsync(int ratingId)
        {
            var rating = await _ratingRepository.GetByIdAsync(ratingId);
            return rating != null ? MapToDto(rating) : null;
        }

        public async Task<List<RatingDto>> GetRatingsByMovieIdAsync(int movieId)
        {
            var ratings = await _ratingRepository.GetByMovieIdAsync(movieId);
            return ratings.Select(MapToDto).ToList();
        }

        public async Task<List<RatingDto>> GetRatingsByUserIdAsync(int userId)
        {
            var ratings = await _ratingRepository.GetByUserIdAsync(userId);
            return ratings.Select(MapToDto).ToList();
        }

        public async Task<RatingDto> GetUserRatingForMovieAsync(int movieId, int userId)
        {
            var rating = await _ratingRepository.GetUserRatingForMovieAsync(movieId, userId);
            return rating != null ? MapToDto(rating) : null;
        }

        public async Task<double> CalculateAverageRatingAsync(int movieId)
        {
            return await _ratingRepository.GetAverageRatingAsync(movieId);
        }

        private async Task UpdateMovieAverageRating(int movieId)
        {
            var averageRating = await _ratingRepository.GetAverageRatingAsync(movieId);
            var movie = await _movieRepository.GetByIdAsync(movieId);
            if (movie != null)
            {
                movie.Rating = averageRating;
                // Lấy danh sách genreIds hiện tại của phim
                var genreIds = movie.Genres?.Select(g => g.GenresId).ToList() ?? new List<int>();
                await _movieRepository.UpdateAsync(movie, genreIds);
            }
        }

        private RatingDto MapToDto(Rating rating)
        {
            return new RatingDto
            {
                RatingId = rating.RatingId,
                UserId = rating.UserId,
                MovieId = rating.MovieId,
                StarRating = rating.StarRating,
                Review = rating.Review,
                CreatedAt = rating.CreatedAt,
                UpdatedAt = rating.UpdatedAt,
                UserName = rating.User?.UserName ?? "Unknown"
            };
        }
    }
}
