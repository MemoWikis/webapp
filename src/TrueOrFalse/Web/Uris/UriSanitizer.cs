using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace TrueOrFalse.Web
{
    public static class UriSanitizer
    {
        public static string Run(string name, int maxLength = 50)
        {
            name = new string(name.Where(IsValidChar)
                                  .SelectMany(Transform)
                                  .Take(maxLength).ToArray());
            return HttpUtility.UrlEncode(name); 
        }

        private static bool IsValidChar(char chr)
        {
            if (Regex.IsMatch(chr.ToString(), "[a-zA-Z0-9-_ ÄäÜüÖö]", RegexOptions.Compiled))
                return true;
            
            return false;
        }

        private static char[] Transform(char chr)
        {
            if (chr == 'ä') return new[] { 'a', 'e'};
            if (chr == 'Ä') return new[] { 'A', 'E' };
            if (chr == 'ü') return new[] { 'u', 'e' };
            if (chr == 'Ü') return new[] { 'U', 'E' };
            if (chr == 'ö') return new[] { 'o', 'e' };
            if (chr == 'Ö') return new[] { 'O', 'E' };

            if (chr == ' ')
                return new[] { '_' };

            return new []{chr};
        }
    }
}