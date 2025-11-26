using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieWebApp.Application.Interfaces;

namespace MovieWebApp.Presentation.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CommentController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly ILogger<CommentController> _logger;

        public CommentController(IAdminService adminService, ILogger<CommentController> logger)
        {
            _adminService = adminService;
            _logger = logger;
        }

        /// <summary>
        /// GET: api/Comment
        /// Lấy tất cả bình luận trong hệ thống
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllComments()
        {
            try
            {
                _logger.LogInformation("Admin getting all comments");
                var comments = await _adminService.GetAllCommentsAsync();
                return Ok(comments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all comments");
                return BadRequest(new { message = "Lỗi khi lấy danh sách bình luận", error = ex.Message });
            }
        }

        /// <summary>
        /// DELETE: api/Comment/{id}
        /// Xóa một bình luận
        /// </summary>
        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            try
            {
                _logger.LogInformation("Admin deleting comment {CommentId}", commentId);
                var result = await _adminService.DeleteCommentAsync(commentId);
                
                if (!result)
                    return NotFound(new { message = "Không tìm thấy bình luận" });

                return Ok(new { message = "Xóa bình luận thành công" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting comment {CommentId}", commentId);
                return BadRequest(new { message = "Lỗi khi xóa bình luận", error = ex.Message });
            }
        }
    }
}
