using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using static System.String;

namespace TrueOrFalse.Web
{
    public static class UriSanitizer
    {
        public static string Run(string name, int maxLength = 50)
        {
            if (IsNullOrEmpty(name))
                name = "_";

            name = new string(name.Where(IsValidChar)
                                  .SelectMany(Transform)
                                  .Take(maxLength).ToArray());

            name = name.Replace("---", "-").Replace("--", "-");

            return HttpUtility.UrlEncode(name); 
        }

        private static bool IsValidChar(char chr)
        {
            if (Regex.IsMatch(chr.ToString(), "[a-zA-Z0-9-_ ÄäÜüÖöß]", RegexOptions.Compiled))
                return true;
            
            return false;
        }

        private static char[] Transform(char chr)
        {
            if (chr == 'ä') return new[] { 'a', 'e'};
            if (chr == 'Ä') return new[] { 'A', 'e' };
            if (chr == 'ü') return new[] { 'u', 'e' };
            if (chr == 'Ü') return new[] { 'U', 'e' };
            if (chr == 'ö') return new[] { 'o', 'e' };
            if (chr == 'Ö') return new[] { 'O', 'e' };
            if (chr == 'ß') return new[] { 's', 's' };

            if (chr == ' ')
                return new[] { '-' };

            if (chr == '_')
                return new[] { '-' };

            return new []{chr};
        }
    }
}