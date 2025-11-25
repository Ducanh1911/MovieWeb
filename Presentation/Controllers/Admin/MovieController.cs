using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieWebApp.Application.DTOs;
using MovieWebApp.Application.Interfaces;

namespace MovieWebApp.Presentation.Controllers.Admin
{
    [Route("api/admin/movies")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllMovies()
        {
            try
            {
                var movies = await _movieService.GetAllMoviesAdminAsync();
                return Ok(new { data = movies });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server khi lấy danh sách phim.", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateMovie([FromForm] MovieUploadRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ.", errors = ModelState });
            }

            try
            {
                var movie = await _movieService.CreateMovieWithFilesAsync(request);
                return Ok(new { message = "Tạo phim thành công.", data = movie });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = "Bạn không có quyền thực hiện thao tác này." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server khi tạo phim.", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(int id, [FromForm] MovieUploadRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ.", errors = ModelState });
            }

            try
            {
                var movie = await _movieService.UpdateMovieWithFilesAsync(id, request);

                if (movie == null)
                {
                    return NotFound(new { message = $"Phim với ID {id} không tồn tại." });
                }
                return Ok(new { message = "Cập nhật phim thành công.", data = movie });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = "Bạn không có quyền thực hiện thao tác này." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server khi cập nhật phim.", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            try
            {
                var deleted = await _movieService.DeleteMovieAsync(id);
                if (!deleted)
                {
                    return NotFound(new { message = $"Phim với ID {id} không tồn tại." });
                }
                return Ok(new { message = "Xóa phim thành công." });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = "Bạn không có quyền thực hiện thao tác này." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server khi xóa phim.", error = ex.Message });
            }
        }
    }
}
