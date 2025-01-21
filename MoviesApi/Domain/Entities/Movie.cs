using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Movie
    {
        public int MovieId { get; set; }
        public required string Title { get; set; }
        public required string ReleaseYear { get; set; }
        public required string PosterImg { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
