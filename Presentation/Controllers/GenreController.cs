using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieWebApp.Application.DTOs;
using MovieWebApp.Application.Interfaces;

namespace MovieWebApp.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _genreService;
        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var genre=await _genreService.GetAllAsync();
            return Ok(genre);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var genre = await _genreService.GetByIdAsync(id);
            if (genre == null) {
                return BadRequest();
            }
            return Ok(genre);
        }
        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateGenreAsync(GenreDto genreDto)
        {
            if (genreDto == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var genre = await _genreService.CreateGenreAsync(genreDto);
            return Ok(genre);
            
        }
        //[HttpPost("add-with-builder")]
        //public async Task<IActionResult> AddGenreWithBuilder([FromBody] string genreName)
        //{
        //    if (string.IsNullOrWhiteSpace(genreName))
        //        return BadRequest("Tên thể loại không được để trống");
        //    var genreDto = new GenreDto.Builder()
        //                        .SetName(genreName)
        //                        .Build();

        //    var genre = await _genreService.CreateGenreAsync(genreDto);

        //    return Ok(genre);
        //}
        [HttpPost("duplicate")]
        public IActionResult DuplicateGenre([FromBody] GenreDto genreDto)
        {
            var clone = (GenreDto)genreDto.Clone();
            clone.Name += " - Clone";

            return Ok(clone);
        }



        //[Authorize]

        [HttpDelete]
        public async Task<IActionResult> DeleteGenreAsync(int id)
        {
            try{
                var genre = await _genreService.DeleteGenreAsync(id);
                if (genre == null) return NotFound();
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }

        }
    }
}
