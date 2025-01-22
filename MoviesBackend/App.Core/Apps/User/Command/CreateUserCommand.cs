using App.Core.Interfaces;
using App.Core.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Apps.User.Command
{
    public class CreateUserCommand : IRequest<object>
    {
        public UserDto User { get; set; }
    }
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, object>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IMapper _mapper;
        public CreateUserCommandHandler(IAppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<object> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var dto = command.User;
            if (dto == null)
            {
                return new
                {
                    status = 404,
                    message = "User Data cannot be null"
                };
            }

            var IsExist = await _appDbContext.Set<Domain.Entities.User>().FirstOrDefaultAsync(x => x.Email == dto.Email);
            if (IsExist != null)
            {
                return new
                {
                    status = 409,
                    message = "Email Already Exist"
                };
            }

            var NewUser = _mapper.Map<Domain.Entities.User>(dto);
            NewUser.Password = HashPassword(dto.Password);

            Guid key = Guid.NewGuid();
            var ApiKey = key.ToString();
            NewUser.ApiKey = ApiKey;

            await _appDbContext.Set<Domain.Entities.User>().AddAsync(NewUser);
            await _appDbContext.SaveChangesAsync();
            return new
            {
                status = 200,
                message = "User Registered Successfully"
            };
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

    }
}
