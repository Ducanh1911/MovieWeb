using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieWebApp.Application.DTOs;
using MovieWebApp.Application.Interfaces;
using System.Security.Claims;

namespace MovieWebApp.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRating([FromBody] CreateRatingDto createRatingDto)
        {
            try
            {
                var userId = GetCurrentUserId();
                var rating = await _ratingService.CreateRatingAsync(createRatingDto, userId);
                return Ok(rating);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Có lỗi xảy ra khi tạo đánh giá" });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRating(int id, [FromBody] UpdateRatingDto updateRatingDto)
        {
            try
            {
                var userId = GetCurrentUserId();
                var rating = await _ratingService.UpdateRatingAsync(id, updateRatingDto, userId);
                return Ok(rating);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Có lỗi xảy ra khi cập nhật đánh giá" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRating(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await _ratingService.DeleteRatingAsync(id, userId);
                if (!result)
                    return NotFound();
                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Có lỗi xảy ra khi xóa đánh giá" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRating(int id)
        {
            var rating = await _ratingService.GetRatingByIdAsync(id);
            if (rating == null)
                return NotFound();
            return Ok(rating);
        }

        [HttpGet("movie/{movieId}")]
        public async Task<IActionResult> GetRatingsByMovie(int movieId)
        {
            var ratings = await _ratingService.GetRatingsByMovieIdAsync(movieId);
            return Ok(ratings);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetRatingsByUser(int userId)
        {
            var ratings = await _ratingService.GetRatingsByUserIdAsync(userId);
            return Ok(ratings);
        }

        [HttpGet("movie/{movieId}/user")]
        public async Task<IActionResult> GetUserRatingForMovie(int movieId)
        {
            var userId = GetCurrentUserId();
            var rating = await _ratingService.GetUserRatingForMovieAsync(movieId, userId);
            return Ok(rating);
        }

        [HttpGet("movie/{movieId}/average")]
        public async Task<IActionResult> GetAverageRating(int movieId)
        {
            var averageRating = await _ratingService.CalculateAverageRatingAsync(movieId);
            return Ok(new { AverageRating = averageRating });
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out int userId))
                return userId;
            throw new UnauthorizedAccessException("Không thể xác định người dùng");
        }
    }
}

