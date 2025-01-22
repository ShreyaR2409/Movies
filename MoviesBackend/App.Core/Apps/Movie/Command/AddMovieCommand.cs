using App.Core.Interfaces;
using App.Core.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Apps.Movie.Command
{
    public class AddMovieCommand : IRequest<object>
    {
        public MovieDto Movie { get; set; }
    }
    public class AddMovieCommandHandler : IRequestHandler<AddMovieCommand, object>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IMapper _mapper;
        public AddMovieCommandHandler(IAppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<object> Handle(AddMovieCommand command, CancellationToken cancellationToken)
        {
            var dto = command.Movie;
            var IsExist = await _appDbContext.Set<Domain.Entities.Movie>().FirstOrDefaultAsync(x => x.Title == dto.Title && x.ReleaseYear == dto.ReleaseYear);
            if (IsExist != null) {
                var response = new
                {
                    status = 409,
                    message = "Movie Already Exist"
                };
                return response;
            }

            string ImgPath = null;

            if (dto.PosterImg != null) {
                ImgPath = await UploadImagesAsync(dto.PosterImg);
            }

            var Movie = _mapper.Map<Domain.Entities.Movie>(dto);
            //var Movie = dto.Adapt<Domain.Entities.Movie>();
            Movie.PosterImg = ImgPath;
            await _appDbContext.Set<Domain.Entities.Movie>().AddAsync(Movie);
            await _appDbContext.SaveChangesAsync();
            return new
            {
                status = 200,
                message = "Movie Added Successfully"
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
            return $"/uploads/{filename}";
        }
    }
}
