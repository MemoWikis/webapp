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
            name = new string(name.Where(IsValidChar)
                                  .Select(Transform)
                                  .Take(maxLength).ToArray());
            return HttpUtility.UrlEncode(name); 
        }

        private static bool IsValidChar(char chr)
        {
            if (Regex.IsMatch(chr.ToString(), "[a-zA-Z0-9-_ ]", RegexOptions.Compiled))
                return true;
            
            return false;
        }

        private static char Transform(char chr)
        {
            if (chr == ' ')
                return '_';

            return chr;
        }
    }
}