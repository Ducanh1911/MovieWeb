using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieWebApp.Application.Interfaces;

namespace MovieWebApp.Presentation.Controllers.Admin
{
    [Route("api/admin/ratings")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RatingController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly ILogger<RatingController> _logger;

        public RatingController(IAdminService adminService, ILogger<RatingController> logger)
        {
            _adminService = adminService;
            _logger = logger;
        }

        /// <summary>
        /// GET: api/Rating
        /// Lấy tất cả đánh giá trong hệ thống
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllRatings()
        {
            try
            {
                _logger.LogInformation("Admin getting all ratings");
                var ratings = await _adminService.GetAllRatingsAsync();
                return Ok(ratings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all ratings");
                return BadRequest(new { message = "Lỗi khi lấy danh sách đánh giá", error = ex.Message });
            }
        }

        /// <summary>
        /// DELETE: api/Rating/{id}
        /// Xóa một đánh giá
        /// </summary>
        [HttpDelete("{ratingId}")]
        public async Task<IActionResult> DeleteRating(int ratingId)
        {
            try
            {
                _logger.LogInformation("Admin deleting rating {RatingId}", ratingId);
                var result = await _adminService.DeleteRatingAsync(ratingId);
                
                if (!result)
                    return NotFound(new { message = "Không tìm thấy đánh giá" });

                return Ok(new { message = "Xóa đánh giá thành công" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting rating {RatingId}", ratingId);
                return BadRequest(new { message = "Lỗi khi xóa đánh giá", error = ex.Message });
            }
        }
    }
}
