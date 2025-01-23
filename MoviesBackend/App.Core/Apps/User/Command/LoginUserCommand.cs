using App.Core.Interfaces;
using App.Core.Models;
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
        public LoginDto loginDto { get; set; }
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
            try
            {
                var dto = command.loginDto;
                var ValidEmail = _appDbContext.Set<Domain.Entities.User>().FirstOrDefault(x => x.Email == dto.Email);
                if (ValidEmail == null || !VerifyPassword(dto.Password, ValidEmail.Password))
                {
                    return new
                    {
                        status = 404,
                        message = "Invalid Email or Password"
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
                    Token = _jwtService.GenarateToken(ValidEmail, role.RoleType, ValidEmail.ApiKey)
                };
            }
            catch (DbUpdateException dbEx)
            {
                // Handle database update exceptions
                return new
                {
                    status = 500,
                    message = "An error occurred while accessing the database.",
                    details = dbEx.Message
                };
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                return new
                {
                    status = 500,
                    message = "An unexpected error occurred.",
                    details = ex.Message
                };
            }
        }
        public bool VerifyPassword(string plainTextPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(plainTextPassword, hashedPassword);
        }

    }
}
