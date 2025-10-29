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
                return Ok(comment);
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
                return Ok(comment);
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
                    return NotFound();
                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Có lỗi xảy ra khi xóa bình luận" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetComment(int id)
        {
            var comment = await _commentService.GetCommentByIdAsync(id);
            if (comment == null)
                return NotFound();
            return Ok(comment);
        }

        [HttpGet("movie/{movieId}")]
        public async Task<IActionResult> GetCommentsByMovie(int movieId)
        {
            var comments = await _commentService.GetCommentsByMovieIdAsync(movieId);
            return Ok(comments);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetCommentsByUser(int userId)
        {
            var comments = await _commentService.GetCommentsByUserIdAsync(userId);
            return Ok(comments);
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

















