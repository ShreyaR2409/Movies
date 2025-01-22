using App.Core.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Validators
{
    public class MovieDtoValidator : AbstractValidator<MovieDto>
    {
        public MovieDtoValidator()
        {
            RuleFor(x => x.Title)
           .NotEmpty().WithMessage("Title is required.")
           .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

            RuleFor(x => x.ReleaseYear)
                .NotEmpty().WithMessage("Release Year is required.")
                .Matches(@"^\d{4}$").WithMessage("Release Year must be a valid 4-digit year.");

            RuleFor(x => x.PosterImg)
                .NotNull().WithMessage("Poster image is required.")
                .Must(file => file.Length > 0).WithMessage("Poster image cannot be empty.")
                .Must(file => IsImageFile(file)).WithMessage("Poster image must be a valid image file (e.g., .jpg, .png).");
        }

        private bool IsImageFile(IFormFile file)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = System.IO.Path.GetExtension(file.FileName)?.ToLower();
            return allowedExtensions.Contains(fileExtension);
        }

    }
}
