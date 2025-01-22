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
        public  string Title { get; set; }
        public  string ReleaseYear { get; set; }
        public  IFormFile? PosterImg { get; set; }
    }
}
