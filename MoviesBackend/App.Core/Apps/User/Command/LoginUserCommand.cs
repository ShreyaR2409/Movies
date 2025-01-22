using App.Core.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Apps.User.Command
{
    public class LoginUserCommand : IRequest<object>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand,object>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IJwtService _jwtService;
        public LoginUserCommandHandler(IAppDbContext appDbContext, IJwtService jwtService)
        {
            _appDbContext = appDbContext;
            _jwtService = jwtService;
        }

        public async Task<object> Handle(LoginUserCommand command, CancellationToken cancellationToken)
        {
            var ValidEmail = _appDbContext.Set<Domain.Entities.User>().FirstOrDefault(x => x.Email == command.Email);
            if (ValidEmail == null)
            {
                return new
                {
                    status = 404,
                    message = "Invalid Email"
                };
            }

            var ValidPassword = _appDbContext.Set<Domain.Entities.User>().FirstOrDefault(x => x.Password == command.Password);
            if (ValidPassword == null)
            {
                return new
                {
                    status = 404,
                    message = "Invalid Password"
                };
            }

            var role = await _appDbContext.Set<Domain.Entities.Role>().FirstOrDefaultAsync(r => r.RoleId == ValidEmail.RoleId);
            if (role == null)
            {
                return new
                {
                    status = 404,
                    message = "Role Not Found"
                };
            }

            return new
            {
                status = 200,
                message = "User Login Successfully",
                Token = _jwtService.GenarateToken(ValidEmail, role.RoleType)
            };
        }
    }
}
