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
        public string Title { get; set; }
        public string ReleaseYear { get; set; }
        public string PosterImg { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
