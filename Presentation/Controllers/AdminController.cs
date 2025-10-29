using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieWebApp.Application.Interfaces;
using MovieWebApp.Application.DTOs;
using System.Security.Claims;
using MovieWebApp.Domain.Interfaces;

namespace MovieWebApp.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly IUserRepository _userRepository;
        private readonly IRatingRepository _ratingRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IFavoriteRepository _favoriteRepository;

        public AdminController(
            IMovieService movieService,
            IUserRepository userRepository,
            IRatingRepository ratingRepository,
            ICommentRepository commentRepository,
            IFavoriteRepository favoriteRepository)
        {
            _movieService = movieService;
            _userRepository = userRepository;
            _ratingRepository = ratingRepository;
            _commentRepository = commentRepository;
            _favoriteRepository = favoriteRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { message = "Chào mừng đến với trang quản trị!" });
        }

        // ==================== DASHBOARD STATISTICS ====================
        //[HttpGet("dashboard")]
        //public async Task<IActionResult> GetDashboardStats()
        //{
        //    try
        //    {
        //        var movies = await _movieService.GetAllMoviesAsync();
        //        var users = await _userRepository.GetAllAsync();
        //        var ratings = await _ratingRepository.GetAllAsync();
        //        var comments = await _commentRepository.GetAllAsync();
        //        var favorites = await _favoriteRepository.GetAllAsync();

        //        //var stats = new
        //        //{
        //        //    TotalMovies = movies.Count(),
        //        //    TotalUsers = users.Count(),
        //        //    TotalRatings = ratings.Count(),
        //        //    TotalComments = comments.Count(),
        //        //    TotalFavorites = favorites.Count(),
        //        //    AverageRating = movies.Any() ? movies.Average(m => m.Rating) : 0,
        //        //    RecentMovies = movies.OrderByDescending(m => m.createdAt).Take(5).Select(m => new
        //        //    {
        //        //        m.MovieId,
        //        //        m.MovieName,
        //        //        m.Poster,
        //        //        m.Rating,
        //        //        m.ViewCount,
        //        //        m.createdAt
        //        //    }),
        //        //    TopRatedMovies = movies.OrderByDescending(m => m.Rating).Take(5).Select(m => new
        //        //    {
        //        //        m.MovieId,
        //        //        m.MovieName,
        //        //        m.Poster,
        //        //        m.Rating,
        //        //        m.ViewCount
        //        //    })
        //        //};

        //        //return Ok(stats);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { message = "Lỗi khi lấy thống kê dashboard", error = ex.Message });
        //    }
        //}

        // ==================== USER MANAGEMENT ====================
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userRepository.GetAllAsync();
                var userDtos = users.Select(u => new
                {
                    u.UserId,
                    u.UserName,
                    u.Email,
                    u.Role,
                    u.CreatedAt,
                    //IsActive = !u.IsDeleted
                });

                return Ok(userDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi lấy danh sách người dùng", error = ex.Message });
            }
        }

        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                    return NotFound(new { message = "Không tìm thấy người dùng" });

                var userDto = new
                {
                    user.UserId,
                    user.UserName,
                    user.Email,
                    user.Role,
                    user.CreatedAt,
                    //IsActive = !user.IsDeleted
                };

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi lấy thông tin người dùng", error = ex.Message });
            }
        }

        [HttpPut("users/{userId}/role")]
        public async Task<IActionResult> UpdateUserRole(int userId, [FromBody] UpdateUserRoleDto dto)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                    return NotFound(new { message = "Không tìm thấy người dùng" });

                user.Role = dto.Role;
                await _userRepository.UpdateAsync(user);

                return Ok(new { message = "Cập nhật vai trò thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi cập nhật vai trò", error = ex.Message });
            }
        }

        [HttpDelete("users/{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            try
            {
                var result = await _userRepository.DeleteAsync(userId);
                if (!result)
                    return NotFound(new { message = "Không tìm thấy người dùng" });

                return Ok(new { message = "Xóa người dùng thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi xóa người dùng", error = ex.Message });
            }
        }

        // ==================== MOVIE MANAGEMENT ====================
        // CRUD phim được cung cấp bởi MovieController. Không triển khai ở AdminController.

        // ==================== RATING & COMMENT MANAGEMENT ====================
        [HttpGet("ratings")]
        public async Task<IActionResult> GetAllRatings()
        {
            try
            {
                var ratings = await _ratingRepository.GetAllAsync();
                var ratingDtos = ratings.Select(r => new
                {
                    r.RatingId,
                    r.UserId,
                    r.MovieId,
                    r.StarRating,
                    r.Review,
                    r.CreatedAt,
                    r.UpdatedAt,
                    UserName = r.User?.UserName,
                    MovieName = r.Movie?.MovieName
                });

                return Ok(ratingDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi lấy danh sách đánh giá", error = ex.Message });
            }
        }

        [HttpDelete("ratings/{ratingId}")]
        public async Task<IActionResult> DeleteRating(int ratingId)
        {
            try
            {
                var result = await _ratingRepository.DeleteAsync(ratingId);
                if (!result)
                    return NotFound(new { message = "Không tìm thấy đánh giá" });

                return Ok(new { message = "Xóa đánh giá thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi xóa đánh giá", error = ex.Message });
            }
        }

        [HttpGet("comments")]
        public async Task<IActionResult> GetAllComments()
        {
            try
            {
                var comments = await _commentRepository.GetAllAsync();
                var commentDtos = comments.Select(c => new
                {
                    c.CommentId,
                    c.UserId,
                    c.MovieId,
                    c.Content,
                    c.CreatedAt,
                    c.UpdatedAt,
                    UserName = c.User?.UserName,
                    MovieName = c.Movie?.MovieName
                });

                return Ok(commentDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi lấy danh sách bình luận", error = ex.Message });
            }
        }

        [HttpDelete("comments/{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            try
            {
                var result = await _commentRepository.DeleteAsync(commentId);
                if (!result)
                    return NotFound(new { message = "Không tìm thấy bình luận" });

                return Ok(new { message = "Xóa bình luận thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi xóa bình luận", error = ex.Message });
            }
        }
    }

    // DTOs for Admin
    public class UpdateUserRoleDto
    {
        public string Role { get; set; }
    }

        public class UpdateMovieDto
        {
            public string? MovieName { get; set; }
            public string? Description { get; set; }
            public int? ReleaseYear { get; set; }
            public string? Country { get; set; }
            public string? Language { get; set; }
            public string? TrailerUrl { get; set; }
            public string? Director { get; set; }
            public string? Cast { get; set; }
            public int? Episodes { get; set; }
            public List<int>? GenreIds { get; set; }
        }
}