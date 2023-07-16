using System;
namespace MovieVerse.Models
{
	public class FilmShortViewModel
	{
		public IEnumerable<FilmShort> Films { get; set; }
        public string TypeTop { get; set; }
        public string TypeTopHeader { get; set; }
		public string SearchKeyword { get; set; }

		public int Page { get; set; }
		public int PagesCount { get; set; }
	}
}

