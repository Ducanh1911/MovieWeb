using Microsoft.AspNetCore.Mvc;
using MovieWebApp.Application.Interfaces;

namespace MovieWebApp.Presentation.Controllers.Client
{
    [Route("api/genres")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var genres = await _genreService.GetAllAsync();
                return Ok(new { data = genres });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server khi lấy danh sách thể loại.", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var genre = await _genreService.GetByIdAsync(id);
                if (genre == null)
                {
                    return NotFound(new { message = "Không tìm thấy thể loại." });
                }
                return Ok(new { data = genre });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server khi lấy thông tin thể loại.", error = ex.Message });
            }
        }
    }
}
