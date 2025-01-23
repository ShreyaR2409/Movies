using App.Core.Interfaces;
using App.Core.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Apps.Movie.Query
{
    public class SearchMovieRequest : IRequest<object>
    {
        public string s {  get; set; }
        public string apikey { get; set; }
    }
    public class SearchMovieRequestHandler : IRequestHandler<SearchMovieRequest, object>
    {
        private readonly IAppDbContext _appDbContext;
        public SearchMovieRequestHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<object> Handle(SearchMovieRequest request, CancellationToken cancellationToken)
        {
            var auth = await _appDbContext.Set<Domain.Entities.User>().FirstOrDefaultAsync(x=> x.ApiKey == request.apikey);
            if (auth == null)
            {
                return new
                {
                    status = 404,
                    message = "Not Authorized"
                };
            }
            var query = _appDbContext.Set<Domain.Entities.Movie>()
                                     .Where(m => m.IsDeleted == false);

            if (!string.IsNullOrWhiteSpace(request.s))
            {
                query = query.Where(x => x.Title != null &&
                                         EF.Functions.Like(x.Title, $"%{request.s}%"));
            }

            var allMovieData = await query.ToListAsync(cancellationToken);
            return new
            {
                Status = 200,
                Message = "Filtered Movie Data",
                Data = allMovieData
            };
        }
    }
}
