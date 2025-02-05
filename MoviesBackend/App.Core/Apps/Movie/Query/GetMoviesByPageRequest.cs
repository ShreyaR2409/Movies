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
    public class GetMoviesByPageRequest : IRequest<object>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string? SortColumn { get; set; }
        public string? SortOrder { get; set; }
        public string? FilterValue { get; set; }
    }
    public class GetMoviesByPageRequestHandler : IRequestHandler<GetMoviesByPageRequest, object> 
    {
        private readonly IAppDbContext _appDbContext;
        public GetMoviesByPageRequestHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<object> Handle(GetMoviesByPageRequest request, CancellationToken cancellationToken)
        {
            //var result = await _appDbContext
            //        .Set<Domain.Entities.Movie>()
            //        .Where(x => x.IsDeleted == false).ToListAsync(cancellationToken);

            var query = _appDbContext.Set<Domain.Entities.Movie>().Select(x => x).Where(x => x.IsDeleted == false);

            if(!string.IsNullOrEmpty(request.FilterValue))
            {
                query = query.Where(c => c.Title.ToLower().Contains(request.FilterValue.ToLower()));
            }

            if (request.SortOrder == null)
            {
                query = query.OrderBy(c => c.MovieId);
            }
            else
            {
                query = request.SortOrder == "desc" ? query.OrderByDescending(c => c.Title) : query.OrderBy(c => c.Title);
            }

            var onePageMovieList = query.Skip(request.PageSize * (request.PageIndex - 1)).Take(request.PageSize);

            var result = await onePageMovieList.ToListAsync();

            var totalcount = await query.CountAsync();

            return new
            {
                status = 200,
                message = "Movies Fetched Successfully",
                Data = result,
                TotalCount = totalcount
            };

        }
    }

}
