using App.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Apps.Movie.Query
{
    public class GetMoviesRequest : IRequest<object>
    {
    }

    public class GetMoviesRequestHandler : IRequestHandler<GetMoviesRequest , object>
    {
        private readonly IAppDbContext _appDbContext;
        public GetMoviesRequestHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<object> Handle(GetMoviesRequest request, CancellationToken cancellationToken)
        {
            var result = await _appDbContext.Set<Domain.Entities.Movie>().ToListAsync(cancellationToken);
            return new
            {
                status = 200,
                message = "",
                Data = result
            };
        }
    }
}
