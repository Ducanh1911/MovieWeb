using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieWebApp.Application.DTOs;
using MovieWebApp.Application.Interfaces;
using MovieWebApp.Application.Services;
using MovieWebApp.Presentation.Request;

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

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var (movies, totalCount) = await _movieService.GetPagedMoviesAsync(pageNumber, pageSize);

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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateMovie([FromBody] MovieUploadRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var dto = new MovieDto
                {
                    MovieName = request.MovieName,
                    Description = request.Description,
                    ReleaseYear = request.ReleaseYear,
                    Country = request.Country,
                    Language = request.Language,
                    Poster = request.Poster,
                    VideoUrl = request.VideoUrl,
                    GenreIds = request.GenreIds,
                };

                var movie = await _movieService.CreateMovieAsync(dto);
                return Ok(movie);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateMovie(int id, [FromBody] MovieDto dto)
        {
            try
            {
                if (!ModelState.IsValid) 
                    return BadRequest(ModelState);
                var movie = await _movieService.UpdateMovieAsync(id, dto);
                if (movie == null) return NotFound();
                return Ok(movie);
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
           var deleted = await _movieService.DeleteMovieAsync(id);
            if (!deleted) return NotFound();
            return Ok(new { success = true });
            
        }

    }
}
