using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieWebApp.Application.DTOs;
using MovieWebApp.Application.Interfaces;
using System.Security.Claims;

namespace MovieWebApp.Presentation.Controllers.Client
{
    [Route("api/favorites")]
    [ApiController]
    [Authorize]
    public class FavoriteController : ControllerBase
    {
        private readonly IFavoriteService _favoriteService;
        private readonly ILogger<FavoriteController> _logger;

        public FavoriteController(IFavoriteService favoriteService, ILogger<FavoriteController> logger)
        {
            _favoriteService = favoriteService;
            _logger = logger;
        }

        private bool TryGetUserId(out int userId)
        {
            userId = 0;
            var claimVal = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(claimVal))
            {
                var all = string.Join(", ", User.Claims.Select(c => $"{c.Type}:{c.Value}"));
                _logger.LogWarning("No userId claim found. Claims: {Claims}", all);
                return false;
            }

            return int.TryParse(claimVal, out userId);
        }

        [HttpGet]
        public async Task<IActionResult> GetFavoritesByUser()
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized(new { message = "Token không chứa userId hợp lệ." });

            var favorites = await _favoriteService.GetFavoritesByUserAsync(userId);
            return Ok(new { data = favorites });
        }

        [HttpPost]
        public async Task<IActionResult> AddFavorite([FromBody] FavoriteDto dto)
        {
            if (dto == null)
                return BadRequest(new { message = "Dữ liệu không hợp lệ" });

            if (!TryGetUserId(out var userId))
                return Unauthorized(new { message = "Token không chứa userId hợp lệ." });

            dto.UserId = userId;

            try
            {
                var favorite = await _favoriteService.AddFavoriteAsync(dto);
                return Ok(new { message = "Thêm vào yêu thích thành công.", data = new { favorite.UserId, favorite.MovieId } });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi thêm vào yêu thích");
                return StatusCode(500, new { message = "Có lỗi xảy ra khi thêm favorite" });
            }
        }

        [HttpDelete("{movieId}")]
        public async Task<IActionResult> RemoveFavorite(int movieId)
        {
            try
            {
                if (!TryGetUserId(out var userId))
                    return Unauthorized(new { message = "Token không chứa userId hợp lệ." });

                var result = await _favoriteService.RemoveFavoriteAsync(userId, movieId);
                if (!result)
                    return NotFound(new { message = "Không tìm thấy trong danh sách yêu thích" });

                return Ok(new { message = "Xóa khỏi yêu thích thành công." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa yêu thích");
                return StatusCode(500, new { message = "Có lỗi xảy ra" });
            }
        }
    }
}
