using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Models
{
    public class MovieDto
    {
        public required string Title { get; set; }
        public required string ReleaseYear { get; set; }
        public required IFormFile PosterImg { get; set; }
    }
}
