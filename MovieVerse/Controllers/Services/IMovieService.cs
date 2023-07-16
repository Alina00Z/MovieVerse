using System;
using System.Threading.Tasks;
using MovieVerse.Models;

namespace MovieVerse.Controllers.Services
{
	public interface IMovieService
	{
        //Task<IEnumerable<FilmShort>> GetMoviesAsync(string url);
        Task<string> GetResponseAsync(string url);
        IEnumerable<FilmShort> ParseMovies(string jsonString);
        int ParsePagesCount(string jsonString);

        Task<Film> GetMovieAsync(string url);

        Task<IEnumerable<FilmInFilter>> GetMoviesFilterAsync(string url);

        Task<List<GenreFilter>> GetGenresFilterAsync(string url);
        Task<List<CountryFilter>> GetCountriesFilterAsync(string url);
        Task<int> FindIDGenreAsync(string url, string genreFind);
        Task<int> FindIDCountryAsync(string url, string countryFind);

        IEnumerable<FilmInFilter> ParseFilmInFilter(string jsonString);
        int ParseTotalPages(string jsonString);
    }
}

