using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieWebApp.Application.DTOs;
using MovieWebApp.Application.Interfaces;
using System.Security.Claims;

namespace MovieWebApp.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class FavoriteController : ControllerBase
    {
        private readonly IFavoriteService _favoriteService;
        private readonly ILogger<FavoriteController> _logger;

        public FavoriteController(IFavoriteService favoriteService, ILogger<FavoriteController> logger)
        {
            _favoriteService = favoriteService;
            _logger = logger;
        }

        // helper an toàn để lấy userId từ token
        private bool TryGetUserId(out int userId)
        {
            userId = 0;
            // kiểm tra cả ClaimTypes.NameIdentifier và "userId" (nếu bạn có thêm "userId" claim)
            var claimVal = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(claimVal))
            {
                // log claims để debug
                var all = string.Join(", ", User.Claims.Select(c => $"{c.Type}:{c.Value}"));
                _logger.LogWarning("No userId claim found. Claims: {Claims}", all);
                return false;
            }

            return int.TryParse(claimVal, out userId);
        }

        [HttpGet]
        public async Task<IActionResult> GetFavoritesByUser()
        {
            if (!TryGetUserId(out var userId)) return Unauthorized("Token không chứa userId hợp lệ.");

            var favorites = await _favoriteService.GetFavoritesByUserAsync(userId);
            return Ok(favorites);
        }

        [HttpPost]
        public async Task<IActionResult> AddFavorite([FromBody] FavoriteDto dto)
        {
            if (dto == null) return BadRequest("Dữ liệu không hợp lệ");

            // lấy userId từ token
            if (!TryGetUserId(out var userId))
                return Unauthorized("Token không chứa userId hợp lệ.");

            dto.UserId = userId;

            try
            {
                var favorite = await _favoriteService.AddFavoriteAsync(dto);
                var response = new { favorite.UserId, favorite.MovieId };
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi thêm vào yêu thích");
                return StatusCode(500, "Có lỗi xảy ra khi thêm favorite");
            }
        }


        [HttpDelete("{movieid}")]
        public async Task<IActionResult> RemoveFavorite(int movieId)
        {
            try {
                if (!TryGetUserId(out var userId)) return Unauthorized("Token không chứa userId hợp lệ.");

                var result = await _favoriteService.RemoveFavoriteAsync(userId, movieId);
                if (!result) return NotFound("Không tìm thấy trong danh sách yêu thích");
            }
            catch
            {

            }

            return NoContent();
        }

        
    }
}
