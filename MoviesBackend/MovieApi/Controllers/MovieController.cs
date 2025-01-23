using App.Core.Apps.Movie.Command;
using App.Core.Apps.Movie.Query;
using App.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
            _logger.LogInformation("Add Movie Method Called with Title: {Title}", newMovie.Title);
            var res = await _mediator.Send(new AddMovieCommand { Movie = newMovie });
            _logger.LogInformation("Response From Add Movie Api", res);
            return Ok(res);
        }

        [HttpGet("Get-Movie")]
        public async Task<IActionResult> GetMovie()
        {
            _logger.LogInformation("Get Movie Method Called");
            var res = await _mediator.Send(new GetMoviesRequest { });
            _logger.LogInformation("Response From Get Movie Api", res);
            return Ok(res);
        }

        [Authorize("Admin")]
        [HttpPut("Update-Movie")]
        public async Task<IActionResult> UpdateMovie( int Id , [FromForm] MovieDto movie)
        {
            _logger.LogInformation("Update Movie Method Called for Movie ID: {MovieId}", Id);
            var res = await _mediator.Send(new  UpdateMovieCommand { Id = Id, Movie = movie });
            _logger.LogInformation("Response From Update Movie Api", res);
            return Ok(res);
        }

        [Authorize("Admin")]
        [HttpDelete("Delete-Movie")]
        public async Task<IActionResult> DeleteMovie(int MovieId)
        {
            _logger.LogInformation("Delete Movie Method Called for Movie ID: {MovieId}", MovieId);
            var res = await _mediator.Send(new DeleteMovieCommand { Id = MovieId });
            _logger.LogInformation("Response From Delete Movie Api", res);
            return Ok(res);
        }

        [HttpGet("getSearchedMovie")]
        public async Task<IActionResult> getAllMovies([FromQuery] string s, string apikey)
        {
             //Validate the API key
            if (string.IsNullOrEmpty(apikey))
            {
                _logger.LogWarning("Unauthorized access attempt with missing API key");
                return Unauthorized(new { Message = "Invalid or missing API key" });
            }
            _logger.LogInformation("Search Movie Method Called with Search Term: {SearchTerm}", s);
            var allMovie = await _mediator.Send(new SearchMovieRequest { s = s, apikey = apikey });
            return Ok(allMovie);
        }
    }
}
