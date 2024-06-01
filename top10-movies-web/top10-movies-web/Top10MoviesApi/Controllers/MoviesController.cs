using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Top10MoviesApi.DTO;
using Top10MoviesApi.Services;

namespace Top10MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovies()
        {
            var movies = await _movieService.GetMoviesAsync();
            return Ok(movies);
        }

        [HttpPost]
        public async Task<ActionResult<MovieDto>> AddMovie(CreateMovieDto createMovieDto)
        {
            try
            {
                var movieDto = await _movieService.AddMovieAsync(createMovieDto);
                return CreatedAtAction(nameof(GetMovies), movieDto);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<IEnumerable<MovieDto>>> UpdateMovie(int id, UpdateMovieDto updateMovieDto)
        {
            try
            {
                var updatedMovies = await _movieService.UpdateMovieAsync(id, updateMovieDto);
                if (updatedMovies == null)
                {
                    return NotFound(new { message = "Movie not found" });
                }
                return Ok(updatedMovies);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
