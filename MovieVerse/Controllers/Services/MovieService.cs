using System;
using System.Net.Http;
using System.Net.Http.Headers;
using MovieVerse.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MovieVerse.Controllers.Services
{
	public class MovieService : IMovieService
	{
        const string API_KEY = "3396d83a-c22a-4190-bcc3-43ca74c23192";


        private readonly HttpClient httpClient;
        public MovieService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("X-API-KEY", API_KEY);
        }
        
        /*public async Task<IEnumerable<FilmShort>> GetMoviesAsync(string url)
		{
            string jsonString = await GetResponseAsync(url);

            var jsonFilmObject = JObject.Parse(jsonString);
            List<FilmShort> films = jsonFilmObject["films"].ToObject<List<FilmShort>>();
            return films;
        }*/
        public IEnumerable<FilmShort> ParseMovies(string jsonString)
        {
            var jsonFilmObject = JObject.Parse(jsonString);
            List<FilmShort> films = jsonFilmObject["films"].ToObject<List<FilmShort>>();
            return films;
        }
        public int ParsePagesCount(string jsonString)
        {
            var jsonFilmObject = JObject.Parse(jsonString);
            int pagesCount = jsonFilmObject["pagesCount"].ToObject<int>();
            if (pagesCount > 20) pagesCount = 20;
            return pagesCount;
        }

        public async Task<Film> GetMovieAsync(string url)
        {
            string jsonString = await GetResponseAsync(url);
            Film film = JsonConvert.DeserializeObject<Film>(jsonString);
            return film;
        }
        public async Task<IEnumerable<FilmInFilter>> GetMoviesFilterAsync(string url)
        {
            string jsonString = await GetResponseAsync(url);

            var jsonFilmObject = JObject.Parse(jsonString);
            List<FilmInFilter> films = jsonFilmObject["items"].ToObject<List<FilmInFilter>>();

            return films;
        }
        public async Task<List<GenreFilter>> GetGenresFilterAsync(string url)
        {
            string jsonString = await GetResponseAsync(url);
            var jsonFilmObject = JObject.Parse(jsonString);
            List<GenreFilter> genres = jsonFilmObject["genres"].ToObject<List<GenreFilter>>();
            return genres;
        }
        public async Task<List<CountryFilter>> GetCountriesFilterAsync(string url)
        {
            string jsonString = await GetResponseAsync(url);
            var jsonFilmObject = JObject.Parse(jsonString);
            List<CountryFilter> countries = jsonFilmObject["countries"].ToObject<List<CountryFilter>>();
            return countries;
        }
        public async Task<int> FindIDGenreAsync(string url, string genreFind)
        {
            List<GenreFilter> genres = await GetGenresFilterAsync(url);
            foreach (GenreFilter genre in genres)
            {
                if (genre.genre == genreFind) return genre.id;
            }
            return 0;
        }
        public async Task<int> FindIDCountryAsync(string url, string countryFind)
        {
            List<CountryFilter> countries = await GetCountriesFilterAsync(url);
            foreach (CountryFilter country in countries)
            {
                if (country.country == countryFind) return country.id;
            }
            return 0;
        }

        public async Task<string> GetResponseAsync(string url)
        {
            //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //httpClient.DefaultRequestHeaders.Add("X-API-KEY", API_KEY);

            // Отправляем GET-запрос и получаем ответ
            HttpResponseMessage response = await httpClient.GetAsync(url);
            // Убедитесь, что запрос выполнен успешно
            response.EnsureSuccessStatusCode();
            string jsonString = await response.Content.ReadAsStringAsync();

            return jsonString;
        }
        public IEnumerable<FilmInFilter> ParseFilmInFilter(string jsonString)
        {
            var jsonStringObject = JObject.Parse(jsonString);
            return jsonStringObject["items"].ToObject<IEnumerable<FilmInFilter>>();
        }
        public int ParseTotalPages(string jsonString)
        {
            var jsonStringObject = JObject.Parse(jsonString);
            return jsonStringObject["totalPages"].ToObject<int>();
        }
    }
}

