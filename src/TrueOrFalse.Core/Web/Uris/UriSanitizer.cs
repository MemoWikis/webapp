using System.Linq;
using System.Web;
using TrueOrFalse.Core.Infrastructure;

namespace TrueOrFalse.Core.Web
{
    public static class UriSanitizer
    {
        public static string Run(string name, int maxLength = 50)
        {
            name = new string(name.Take(maxLength).ToArray());
            return HttpUtility.UrlEncode(name.Replace(" ", "_")); 
        }
    }
}