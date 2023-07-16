using System;
namespace MovieVerse.Models
{
	public class FilmsInFilterViewModel
	{
		public IEnumerable<FilmInFilter> Films { get; set; }

        public int Page { get; set; }
        public int TotalPages { get; set; }
    }
}

