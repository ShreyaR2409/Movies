using App.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Apps.Movie.Command
{
    public class DeleteMovieCommand : IRequest<object>
    {
        public int Id { get; set; }
    }
    public class DeleteMovieCommandHandler : IRequestHandler<DeleteMovieCommand, object>
    {
        private readonly IAppDbContext _appDbContext;
        public DeleteMovieCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<object> Handle(DeleteMovieCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var IsExist = await _appDbContext.Set<Domain.Entities.Movie>().FirstOrDefaultAsync(x => x.MovieId == command.Id && x.IsDeleted == false);
                if (IsExist == null)
                {
                    return new
                    {
                        status = 404,
                        message = "Id not present or Already Deleted"

                    };
                }

                IsExist.IsDeleted = true;
                _appDbContext.Set<Domain.Entities.Movie>().Update(IsExist);
                await _appDbContext.SaveChangesAsync();
                return new
                {
                    status = 200,
                    message = "Movie Deleted Successfully"
                };
            }
            catch (DbUpdateException dbEx)
            {
                // Handle database update exceptions
                return new
                {
                    status = 500,
                    message = "An error occurred while updating the database.",
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

    }
}
