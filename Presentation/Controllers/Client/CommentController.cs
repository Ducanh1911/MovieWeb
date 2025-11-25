using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieWebApp.Application.DTOs;
using MovieWebApp.Application.Interfaces;
using System.Security.Claims;

namespace MovieWebApp.Presentation.Controllers.Client
{
    [Route("api/comments")]
    [ApiController]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentDto createCommentDto)
        {
            try
            {
                var userId = GetCurrentUserId();
                var comment = await _commentService.CreateCommentAsync(createCommentDto, userId);
                return Ok(new { message = "Tạo bình luận thành công.", data = comment });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Có lỗi xảy ra khi tạo bình luận" });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] UpdateCommentDto updateCommentDto)
        {
            try
            {
                var userId = GetCurrentUserId();
                var comment = await _commentService.UpdateCommentAsync(id, updateCommentDto, userId);
                return Ok(new { message = "Cập nhật bình luận thành công.", data = comment });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = "Bạn không có quyền cập nhật bình luận này." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Có lỗi xảy ra khi cập nhật bình luận" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await _commentService.DeleteCommentAsync(id, userId);
                if (!result)
                    return NotFound(new { message = "Không tìm thấy bình luận." });
                return Ok(new { message = "Xóa bình luận thành công." });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = "Bạn không có quyền xóa bình luận này." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Có lỗi xảy ra khi xóa bình luận" });
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetComment(int id)
        {
            var comment = await _commentService.GetCommentByIdAsync(id);
            if (comment == null)
                return NotFound(new { message = "Không tìm thấy bình luận." });
            return Ok(new { data = comment });
        }

        [HttpGet("movie/{movieId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCommentsByMovie(int movieId)
        {
            var comments = await _commentService.GetCommentsByMovieIdAsync(movieId);
            return Ok(new { data = comments });
        }

        [HttpGet("user/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCommentsByUser(int userId)
        {
            var comments = await _commentService.GetCommentsByUserIdAsync(userId);
            return Ok(new { data = comments });
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
