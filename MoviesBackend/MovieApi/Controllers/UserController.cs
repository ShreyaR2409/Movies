using App.Core.Apps.User.Command;
using App.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace MovieApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UserController> _logger;
        public UserController(IMediator mediator, ILogger<UserController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser(UserDto userDto)
        {
            _logger.LogInformation("Register User Method Called with this Email: {Email}", userDto.Email);
            var res = await _mediator.Send( new CreateUserCommand { User = userDto} );
            _logger.LogInformation("Response From Register User Api", res);
            return Ok(res);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            _logger.LogInformation("Login Method Called for Email: {Email}", loginDto.Email);
            var res = await _mediator.Send(new LoginUserCommand { loginDto = loginDto });
            _logger.LogInformation("Response From Login User Api", res);
            return Ok(res);
        }
    }
}
