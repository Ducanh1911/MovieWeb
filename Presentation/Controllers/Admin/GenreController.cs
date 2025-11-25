using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieWebApp.Application.DTOs;
using MovieWebApp.Application.Interfaces;

namespace MovieWebApp.Presentation.Controllers.Admin
{
    [Route("api/admin/genres")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateGenre(GenreDto genreDto)
        {
            if (genreDto == null)
                return BadRequest(new { message = "Dữ liệu không hợp lệ." });

            if (!ModelState.IsValid)
                return BadRequest(new { message = "Dữ liệu không hợp lệ.", errors = ModelState });

            try
            {
                var genre = await _genreService.CreateGenreAsync(genreDto);
                return Ok(new { message = "Tạo thể loại thành công.", data = genre });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = "Bạn không có quyền thực hiện thao tác này." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server khi tạo thể loại.", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            try
            {
                var genre = await _genreService.DeleteGenreAsync(id);
                if (genre == null)
                    return NotFound(new { message = "Không tìm thấy thể loại." });

                return Ok(new { message = "Xóa thể loại thành công." });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = "Bạn không có quyền thực hiện thao tác này." });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server khi xóa thể loại.", error = ex.Message });
            }
        }
    }
}
