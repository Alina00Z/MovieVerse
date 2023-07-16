using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using MovieVerse.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using MovieVerse.Controllers.Services;
using System.Net.Http;

namespace MovieVerse.Controllers
{
    public class HomeController : Controller
    {
        //private readonly HttpClient httpClient;
        private readonly IMovieService _movieService;
        public HomeController(IMovieService movieService)
        {
            //httpClient = new HttpClient();
            _movieService = movieService;
        }
        const string API_KEY = "3396d83a-c22a-4190-bcc3-43ca74c23192";

        const string REQUEST_URL_FILMS = "https://kinopoiskapiunofficial.tech/api/v2.2/films?";
        const string REQUEST_URL_TOP = "https://kinopoiskapiunofficial.tech/api/v2.2/films/top?";
        const string REQUEST_URL_SEARCH_BY_KEYWORD = "https://kinopoiskapiunofficial.tech/api/v2.1/films/search-by-keyword?";
        const string REQUEST_URL_FILM_BY_ID = "https://kinopoiskapiunofficial.tech/api/v2.2/films/";
        const string REQUEST_URL_FILTERS_GENRES_COUNTRIES = "https://kinopoiskapiunofficial.tech/api/v2.2/films/filters";


        //public async Task<IActionResult> GetMoviesTop(string type, int page)
        //{
        //    string requestUrl = REQUEST_URL_TOP + $"type={type}&page={page}";
        //    return RedirectToAction("GetMovies", "Home", new { url = requestUrl });

        //}
        public async Task<IActionResult> GetMoviesFilters(string? countries, string? genres, string order, string type, int ratingFrom, int ratingTo, int yearFrom, int yearTo, string keyword, int page)
        {
            string jsonString = await _movieService.GetResponseAsync(REQUEST_URL_FILTERS_GENRES_COUNTRIES);
            var jsonStringObject = JObject.Parse(jsonString);
            List<CountryFilter> countriesFilter = jsonStringObject["countries"].ToObject<List<CountryFilter>>();
            List<GenreFilter> genresFilter = jsonStringObject["genres"].ToObject<List<GenreFilter>>();
            CountryFilter countryFind = countriesFilter.FirstOrDefault(c => c.country == countries);
            if (countryFind != null)
            {
                ViewBag.Countries = countryFind.country;
                ViewBag.CountriesID = countryFind.id;
            }
            GenreFilter genreFind = genresFilter.FirstOrDefault(g => g.genre == genres);
            if (genreFind != null)
            {
                ViewBag.Genres = genreFind.genre;
                ViewBag.GenresID = genreFind.id;
            }
            ViewBag.Order = order;
            ViewBag.Type = type;
            ViewBag.RatingFrom = ratingFrom;
            ViewBag.RatingTo = ratingTo;
            ViewBag.YearFrom = yearFrom;
            ViewBag.YearTo = yearTo;
            ViewBag.Keyword = keyword;
            ViewBag.Page = page;

            ViewBag.HrefData = $"GetMoviesFilters?countries={ViewBag.Countries}&genres={ViewBag.Genres}&order={order}&type={type}&ratingFrom={ratingFrom}&ratingTo={ratingTo}&yearFrom={yearFrom}&yearTo={yearTo}&keyword={keyword}";


            string url = REQUEST_URL_FILMS;
            bool textHas = false;
            if (countryFind != null)
            {
                url += $"countries={ViewBag.CountriesID}";
                textHas = true;
            }
            if (genreFind != null)
            {
                if (textHas) url += "&";
                url += $"genres={ViewBag.GenresID}";
                textHas = true;
            }
            if (textHas) url += "&";
            url += $"order={order}&type={type}&ratingFrom={ratingFrom}&ratingTo={ratingTo}&yearFrom={yearFrom}&yearTo={yearTo}";
            if (keyword != "" && keyword != null)
            {
                url += $"&keyword={keyword}";
            }
            url += $"&page={page}";
            string jsonStringFilms = await _movieService.GetResponseAsync(url);
            var films = _movieService.ParseFilmInFilter(jsonStringFilms);
            int totalPages = _movieService.ParseTotalPages(jsonStringFilms);
            //var films = await _movieService.GetMoviesFilterAsync(url);

            FilmsInFilterViewModel filmsInFilterViewModel = new FilmsInFilterViewModel(){ Films = films, Page = page, TotalPages = totalPages };
            return View(filmsInFilterViewModel);
        }
        public async Task<IActionResult> GetMovie(int id)
        {
            var film = await _movieService.GetMovieAsync(REQUEST_URL_FILM_BY_ID + id);
            return View(film);
        }
        public async Task<IActionResult> GetMoviesSearch(string searchKeyword)
        {
            return RedirectToAction("GetMovies", "Home", new { page = 1, searchKeyword = searchKeyword});
        }
        public async Task<IActionResult> GetTop100Movies(int page)
        {
            return RedirectToAction("GetMovies", "Home", new { page = page, typeTop = "TOP_100_POPULAR_FILMS", pagesCount = 5 });
        }
        public async Task<IActionResult> GetTopAwaitMovies(int page)
        {
            return RedirectToAction("GetMovies", "Home", new { page = page, typeTop = "TOP_AWAIT_FILMS"});
        }
        public async Task<IActionResult> GetMovies(int page, string typeTop = null, string searchKeyword = null, int pagesCount = 0)
        {
            string url = "";
            if (typeTop != null)
            {
                url = REQUEST_URL_TOP + $"type={typeTop}&page={page}";
            }
            else if (searchKeyword != null)
            {
                url = REQUEST_URL_SEARCH_BY_KEYWORD + $"keyword={searchKeyword}&page={page}";
            }
            string jsonString = await _movieService.GetResponseAsync(url);
            var films = _movieService.ParseMovies(jsonString);
            if (pagesCount == 0)
            {
                pagesCount = _movieService.ParsePagesCount(jsonString);
            }
            string typeTopHeader = "";
            if (typeTop != null)
            {
                if (typeTop == "TOP_100_POPULAR_FILMS") typeTopHeader = "Топ 100";
                else if (typeTop == "TOP_AWAIT_FILMS") typeTopHeader = "Ожидаемые";
            }
            var filmShortViewModel = new FilmShortViewModel { Films = films, TypeTop = typeTop, TypeTopHeader = typeTopHeader, SearchKeyword = searchKeyword, PagesCount = pagesCount, Page = page };
            // Возвращаем результат действия
            //int startIndexOfType = url.IndexOf("type=") + 5;
            //int endIndexOfType = url.IndexOf('&') - 1;
            //string type = url.Substring(startIndexOfType, endIndexOfType - startIndexOfType + 1);
            //ViewBag.Type = type;

            return View(filmShortViewModel);
        }


        //public async Task<IActionResult> GetMoviesAsync(string url)
        //{
        //    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    httpClient.DefaultRequestHeaders.Add("X-API-KEY", API_KEY);

        //    // Отправляем GET-запрос и получаем ответ
        //    HttpResponseMessage response = await httpClient.GetAsync(url);
        //    // Убедитесь, что запрос выполнен успешно
        //    response.EnsureSuccessStatusCode();
        //    string jsonString = await response.Content.ReadAsStringAsync();


        //    // !! нужно преобразовать в класс фильма
        //    //FilmShort[] films = JsonConvert.DeserializeObject<FilmShort[]>(jsonString);
        //    var jsonFilmObject = JObject.Parse(jsonString);
        //    List<FilmShort> films_films = new List<FilmShort>();
        //    if (jsonFilmObject.Values("films") != null)
        //    {
        //        films_films = jsonFilmObject["films"].ToObject<List<FilmShort>>();
        //    }

        //    // Возвращаем результат действия
        //    int startIndexOfType = url.IndexOf("type=") + 5;
        //    int endIndexOfType = url.IndexOf('&') - 1;
        //    string type = url.Substring(startIndexOfType, endIndexOfType - startIndexOfType + 1);
        //    ViewBag.Type = type;

        //    if (films_films.Count > 0) return View(films_films);
        //    else return View();
        //}

        public async Task<IActionResult> Index()
        {
            //return RedirectToAction("GetTop100Movies", "Home", new { page = 1 });
            return RedirectToAction("GetMoviesFilters", "Home", new
            {
                order = "RATING",
                type = "ALL",
                ratingFrom = 0,
                ratingTo = 10,
                yearFrom = 1000,
                yearTo = 3000,
                keyword = "",
                page = 1
            });

        }


    }
}
