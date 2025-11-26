using Microsoft.EntityFrameworkCore;
using MovieWebApp.Application.DTOs;
using MovieWebApp.Application.Interfaces;
using MovieWebApp.Domain.Interfaces;
using MovieWebApp.Infrastructure.Data;

namespace MovieWebApp.Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRatingRepository _ratingRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly ApplicationDbContext _context;

        public AdminService(
            IUserRepository userRepository,
            IRatingRepository ratingRepository,
            ICommentRepository commentRepository,
            IFavoriteRepository favoriteRepository,
            ApplicationDbContext context)
        {
            _userRepository = userRepository;
            _ratingRepository = ratingRepository;
            _commentRepository = commentRepository;
            _favoriteRepository = favoriteRepository;
            _context = context;
        }

        public async Task<DashboardStatsDto> GetDashboardStatsAsync()
        {
            var movies = await _context.movies.Where(m => !m.IsDeleted).ToListAsync();
            var users = await _userRepository.GetAllAsync();
            var ratings = await _ratingRepository.GetAllAsync();
            var comments = await _commentRepository.GetAllAsync();
            var favorites = await _favoriteRepository.GetAllAsync();

            var averageRating = ratings.Any() ? ratings.Average(r => r.StarRating) : 0;

            var recentMovies = movies
                .OrderByDescending(m => m.createdAt)
                .Take(5)
                .Select(m => new MovieSummaryDto
                {
                    MovieId = m.MovieId,
                    MovieName = m.MovieName,
                    Poster = m.Poster,
                    Rating = m.Rating,
                    ViewCount = m.ViewCount,
                    CreatedAt = m.createdAt
                }).ToList();

            var topRatedMovies = movies
                .Where(m => m.Rating > 0)
                .OrderByDescending(m => m.Rating)
                .ThenByDescending(m => m.ViewCount)
                .Take(5)
                .Select(m => new MovieSummaryDto
                {
                    MovieId = m.MovieId,
                    MovieName = m.MovieName,
                    Poster = m.Poster,
                    Rating = m.Rating,
                    ViewCount = m.ViewCount,
                    CreatedAt = m.createdAt
                }).ToList();

            return new DashboardStatsDto
            {
                TotalMovies = movies.Count,
                TotalUsers = users.Count,
                TotalRatings = ratings.Count,
                TotalComments = comments.Count,
                TotalFavorites = favorites.Count,
                AverageRating = Math.Round(averageRating, 2),
                RecentMovies = recentMovies,
                TopRatedMovies = topRatedMovies
            };
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(u => new UserDto
            {
                UserId = u.UserId,
                UserName = u.UserName,
                Email = u.Email,
                Role = u.Role,
                CreatedAt = u.CreatedAt
            }).ToList();
        }

        public async Task<UserDto?> GetUserByIdAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return null;

            return new UserDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<bool> UpdateUserRoleAsync(int userId, string role)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            user.Role = role;
            await _userRepository.UpdateAsync(user);
            return true;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            return await _userRepository.DeleteAsync(userId);
        }

        public async Task<List<RatingAdminDto>> GetAllRatingsAsync()
        {
            var ratings = await _context.Ratings
                .Include(r => r.User)
                .Include(r => r.Movie)
                .ToListAsync();

            return ratings.Select(r => new RatingAdminDto
            {
                RatingId = r.RatingId,
                UserId = r.UserId,
                MovieId = r.MovieId,
                StarRating = r.StarRating,
                Review = r.Review,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt,
                UserName = r.User?.UserName,
                MovieName = r.Movie?.MovieName
            }).ToList();
        }

        public async Task<bool> DeleteRatingAsync(int ratingId)
        {
            return await _ratingRepository.DeleteAsync(ratingId);
        }

        public async Task<List<CommentAdminDto>> GetAllCommentsAsync()
        {
            var comments = await _context.Comments
                .Include(c => c.User)
                .Include(c => c.Movie)
                .ToListAsync();

            return comments.Select(c => new CommentAdminDto
            {
                CommentId = c.CommentId,
                UserId = c.UserId,
                MovieId = c.MovieId,
                Content = c.Content,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
                UserName = c.User?.UserName,
                MovieName = c.Movie?.MovieName
            }).ToList();
        }

        public async Task<bool> DeleteCommentAsync(int commentId)
        {
            return await _commentRepository.DeleteAsync(commentId);
        }
    }
}
