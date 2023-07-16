using MovieVerse.Models;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace MovieVerse.Controllers
{
    public class Utils
    {
        public static string GetClassColorByRating(double rating)
        {
            if (rating >= 7.0) return "green";
            else if (rating >= 5.0) return "orange";
            else return "red";
        }
        public static string SwitchNotNull(string nameRu, string nameEn)
        {
            if (nameRu != "" && nameRu != null)
            {
                return nameRu;
            }
            else return nameEn;
        }
        
    }
}
