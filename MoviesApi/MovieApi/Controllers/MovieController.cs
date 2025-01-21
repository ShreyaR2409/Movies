using App.Core.Apps.Movie.Command;
using App.Core.Models;
using MediatR;
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
        public MovieController(IMediator mediator, ILogger<MovieController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("Add-Movie")]
        public async Task<IActionResult> AddMovie([FromForm] MovieDto newMovie)
        {
            _logger.LogInformation("Add Movie Method Called");
            var res = await _mediator.Send(new AddMovieCommand { Movie = newMovie });
            return Ok(res);
        }

        [HttpPut("Update-Movie")]
        public async Task<IActionResult> UpdateMovie( int Id , [FromForm] MovieDto movie)
        {
            _logger.LogInformation("Update Movie Method Called");
            var res = await _mediator.Send(new  UpdateMovieCommand { Id = Id, Movie = movie });
            return Ok(res);
        }

        [HttpDelete("Delete-Movie")]
        public async Task<IActionResult> DeleteMovie(int MovieId)
        {
            var res = await _mediator.Send(new DeleteMovieCommand { Id = MovieId });
            return Ok(res);
        }
    }
}
