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
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser(UserDto userDto)
        {
            var res = await _mediator.Send( new CreateUserCommand { User = userDto} );
            return Ok(res);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(string Email, string Password)
        {
            var result = await _mediator.Send(new LoginUserCommand { Email = Email, Password = Password });
            return Ok(result);
        }
    }
}
