namespace MovieVerse.Models
{
    public record class FilmShort(int filmId, string nameRu, string nameEn, string year, string filmLength, Country[] countries, Genre[] genres, string rating, int ratingVoteCount, string posterUrl, string posterUrlPreview, string ratingChange);


    public record class FilmInFilter(int? kinopoiskId, string? imdbId, string nameRu, string nameEn, string nameOriginal, Country[] countries, Genre[]? genres, string ratingKinopoisk, double? ratingImdb, int year, string type, string posterUrl, string posterUrlPreview);

    public record class Film(int? kinopoiskId, string? imdbId, string? nameRu, string? nameEn, string? nameOriginal, string posterUrl, string posterUrlPreview, string coverUrl, string logoUrl, int reviewsCount, string? ratingGoodReview, int ratingGoodReviewVoteCount, double? ratingKinopoisk, int ratingKinopoiskVoteCount, double? ratingImdb, int ratingImdbVoteCount, string? ratingFilmCritics, int ratingFilmCriticsVoteCount, double? ratingAwait, int ratingAwaitCount, double? ratingRfCritics, int ratingRfCriticsVoteCount, string? webUrl, int? year, int? filmLength, string? slogan, string? description, string? shortDescription, string? editorAnnotation, bool isTicketsAvailable, string? productionStatus, string? type, string? ratingMpaa, string? ratingAgeLimits, Country[]? countries, Genre[]? genres, string? startYear, string? endYear, bool serial, bool shortFilm, bool completed, bool hasImax, bool has3D, string? lastSync);

    public record class GenreFilter(int id, string genre);
    public record class CountryFilter(int id, string country);


    public record class Genre(string genre);
    public record class Country(string country);
}
