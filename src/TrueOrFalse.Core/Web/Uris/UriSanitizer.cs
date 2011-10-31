using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using TrueOrFalse.Core.Infrastructure;

namespace TrueOrFalse.Core.Web
{
    public static class UriSanitizer
    {
        public static string Run(string name, int maxLength = 50)
        {
            name = new string(name.Where(IsValidChar).Take(maxLength).ToArray());
            return HttpUtility.UrlEncode(name.Replace(" ", "_")); 
        }

        private static bool IsValidChar(char chr)
        {
            if (Regex.IsMatch(chr.ToString(), "[a-zA-Z0-9-_]", RegexOptions.Compiled))
                return true;
            
            return false;
        }
    }
}