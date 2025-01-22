using App.Core.Apps.Movie.Command;
using App.Core.Apps.Movie.Query;
using App.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MovieApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<MovieController> _logger;
        //private readonly 
        public MovieController(IMediator mediator, ILogger<MovieController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [Authorize("Admin")]
        [HttpPost("Add-Movie")]
        public async Task<IActionResult> AddMovie([FromForm] MovieDto newMovie)
        {
            _logger.LogInformation("Add Movie Method Called");
            var res = await _mediator.Send(new AddMovieCommand { Movie = newMovie });
            return Ok(res);
        }

        [HttpGet("Get-Movie")]
        public async Task<IActionResult> GetMovie()
        {
            var res = await _mediator.Send(new GetMoviesRequest { });
            return Ok(res);
        }

        [Authorize("Admin")]
        [HttpPut("Update-Movie")]
        public async Task<IActionResult> UpdateMovie( int Id , [FromForm] MovieDto movie)
        {
            _logger.LogInformation("Update Movie Method Called");
            var res = await _mediator.Send(new  UpdateMovieCommand { Id = Id, Movie = movie });
            return Ok(res);
        }

        [Authorize("Admin")]
        [HttpDelete("Delete-Movie")]
        public async Task<IActionResult> DeleteMovie(int MovieId)
        {
            var res = await _mediator.Send(new DeleteMovieCommand { Id = MovieId });
            return Ok(res);
        }

        [HttpGet("getSearchedMovie")]
        public async Task<IActionResult> getAllMovies([FromQuery] string s, string apikey)
        {
             //Validate the API key
            if (string.IsNullOrEmpty(apikey))
            {
                return Unauthorized(new { Message = "Invalid or missing API key" });
            }

            // Call the query with the search string
            var allMovie = await _mediator.Send(new SearchMovieRequest { s = s });
            return Ok(allMovie);
        }
    }
}
