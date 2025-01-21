using App.Core.Interfaces;
using App.Core.Models;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace App.Core.Apps.Movie.Command
{
    public class UpdateMovieCommand : IRequest<object>
    {
        public int Id { get; set; }
        public MovieDto Movie { get; set; }
    }

    public class UpdateMovieCommandHandler : IRequestHandler<UpdateMovieCommand, object>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IMapper _mapper;
        public UpdateMovieCommandHandler(IAppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<object> Handle(UpdateMovieCommand command, CancellationToken cancellationToken)
        {
            var dto = command.Movie;
            if (dto == null)
            {
                return new
                {
                    status = 404,
                    message = "Movie  Data Cannot be null"
            };
        }

            var IsExist = await _appDbContext.Set<Domain.Entities.Movie>().FirstOrDefaultAsync(p => p.MovieId == command.Id);
            if (IsExist == null)
            {
                return new
                {
                    status = 404,
                    message = "Movie with this Id not found"
                };
            }

            string ImgPath = null;

            if (dto.PosterImg != null)
            {
                ImgPath = await UploadImagesAsync(dto.PosterImg);
            }

            var Movie = _mapper.Map<Domain.Entities.Movie>(IsExist);
            //var Movie = dto.Adapt<Domain.Entities.Movie>();
            //_mapper.Map(dto, IsExist);
            IsExist.PosterImg = ImgPath;
            _appDbContext.Set<Domain.Entities.Movie>().Update(IsExist);
            await _appDbContext.SaveChangesAsync();
            return new
            {
                status = 200,
                message = "Movie Updated Successfully"
            }; 

        }

        private async Task<string?> UploadImagesAsync(IFormFile profileimage)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string filename = Guid.NewGuid().ToString() + "_" + profileimage.FileName;
            string filePath = Path.Combine(uploadsFolder, filename);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                profileimage.CopyTo(stream);
            }
            return $"/MoviePoster/{filename}";
        }
    }
}
