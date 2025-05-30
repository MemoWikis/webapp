using System.Text.RegularExpressions;
using System.Web;
using static System.String;

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
        if (Regex.IsMatch(chr.ToString(), "[a-zA-Z0-9-_ �������]", RegexOptions.Compiled))
            return true;

        return false;
    }

    private static char[] Transform(char chr)
    {
        if (chr == '�') return new[] { 'a', 'e' };
        if (chr == '�') return new[] { 'A', 'e' };
        if (chr == '�') return new[] { 'u', 'e' };
        if (chr == '�') return new[] { 'U', 'e' };
        if (chr == '�') return new[] { 'o', 'e' };
        if (chr == '�') return new[] { 'O', 'e' };
        if (chr == '�') return new[] { 's', 's' };

        if (chr == ' ')
            return new[] { '-' };

        if (chr == '_')
            return new[] { '-' };

        return new[] { chr };
    }
}