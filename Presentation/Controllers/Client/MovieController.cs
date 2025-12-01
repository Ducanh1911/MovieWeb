using Microsoft.AspNetCore.Mvc;
using MovieWebApp.Application.DTOs;
using MovieWebApp.Application.Interfaces;

namespace MovieWebApp.Presentation.Controllers.Client
{
    [Route("api/movies")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var movies = await _movieService.GetAllMoviesClientAsync();
                return Ok(new { data = movies });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server khi lấy danh sách phim.", error = ex.Message });
            }
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string search = "", [FromQuery] string genre = "")
        {
            try
            {
                var (movies, totalCount) = await _movieService.GetPagedMoviesAsync(pageNumber, pageSize, search, genre);

                var response = new
                {
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                    Data = movies
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server khi lấy danh sách phim.", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var movie = await _movieService.GetMovieByIdAsync(id);
                if (movie == null)
                {
                    return NotFound(new { message = "Không tìm thấy phim." });
                }
                return Ok(new { data = movie });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server khi lấy thông tin phim.", error = ex.Message });
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<MovieDto>>> SearchMovies([FromQuery] string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                {
                    return BadRequest(new { message = "Từ khóa tìm kiếm không được để trống." });
                }

                var movies = await _movieService.SearchMoviesAsync(keyword);
                return Ok(new { data = movies });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server khi tìm kiếm phim.", error = ex.Message });
            }
        }
    }
}
