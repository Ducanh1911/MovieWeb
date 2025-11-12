using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieWebApp.Application.DTOs;
using MovieWebApp.Application.Interfaces;
using MovieWebApp.Application.Services;
using MovieWebApp.Infrastructure.SeedWorks;

namespace MovieWebApp.Presentation.Controllers
{
    [Route("api/[controller]")]
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
            var movie = await _movieService.GetAllMoviesAsync();
            return Ok(movie);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var movie = await _movieService.GetMovieByIdAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return Ok(movie);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<MovieDto>>> SearchMovies([FromQuery] string keyword)
        {
            var movies = await _movieService.SearchMoviesAsync(keyword);
            return Ok(movies);
        }

        //[HttpGet("paged")]
        //public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        //{
        //    var (movies, totalCount) = await _movieService.GetPagedMoviesAsync(pageNumber, pageSize);

        //    var response = new
        //    {
        //        TotalCount = totalCount,
        //        PageNumber = pageNumber,
        //        PageSize = pageSize,
        //        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
        //        Data = movies
        //    };
        //    return Ok(response);
        //}

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateMovie([FromBody] MovieDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ.", errors = ModelState });
            }

            try
            {
                var movie = await _movieService.CreateMovieAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = movie.MovieId }, movie);
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
        //public async Task<ApiResult<MovieDto>> CreateMovie([FromBody] MovieDto request)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return new ApiResult<MovieDto>(false, "lỗi", 400);
        //    }

        //    try
        //    {
        //        var movie = await _movieService.CreateMovieAsync(request);
        //        return new ApiResult<MovieDto>(true, "thêm phim thành công", 400);

        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return new ApiResult<MovieDto>(false, ex.Message, 400);

        //    }
        //    catch (UnauthorizedAccessException ex)
        //    {
        //        return new ApiResult<MovieDto>(false, ex.Message, 401);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ApiResult<MovieDto>(false, "LỖi khi tạo phim", 500);

        //    }
        //}

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateMovie(int id, [FromBody] MovieDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ.", errors = ModelState });
            }

            try
            {
                var movie = await _movieService.UpdateMovieAsync(id, dto);
                if (movie == null)
                {
                    return NotFound(new { message = $"Phim với ID {id} không tồn tại." });
                }
                return Ok(movie);
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
        [Authorize(Roles = "Admin")]
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
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server khi xóa phim.", error = ex.Message });
            }

        }

    }
}
