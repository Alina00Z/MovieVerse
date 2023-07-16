using Microsoft.Extensions.DependencyInjection;
using MovieVerse.Controllers.Services;

namespace MovieVerse
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            // ��������� �������, ��������� � HttpClient, � ��� ����� IHttpClientFactory
            builder.Services.AddHttpClient();
            builder.Services.AddScoped<IMovieService, MovieService>();
            // ������� ��������� ��������
            var serviceProvider = builder.Services.BuildServiceProvider();
            // �������� ������ IHttpClientFactory
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            // ������� ������ HttpClient
            var httpClient = httpClientFactory?.CreateClient();

            var app = builder.Build();
            app.UseStaticFiles();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                    name: "getMoviesTopByPage",
                    pattern: "{controller}/{action}/{page=1}");
            app.Run();
        }
    }
}