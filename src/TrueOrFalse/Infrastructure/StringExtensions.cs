using System.Text.RegularExpressions;

public static class StringExtensions
{
    public static string Truncate(this string input, int maxLength, bool addEllipsis = false)
    {
        if (string.IsNullOrEmpty(input))
            return input;
        if (addEllipsis)
            return input.Length <= maxLength ? input : input.Substring(0, maxLength - 3) + "...";
        return input.Length <= maxLength ? input : input.Substring(0, maxLength);
    }
    
    public static string TruncateAtWord(this string input, int length)
    {
        return input.TruncateAtWordWithEllipsisText(length, "...");
    }

    public static string TruncateAtWordWithEllipsisText(this string input, int length, string ellipsisText)
    {
        if (input == null || input.Length < length)
            return input;
        int iNextSpace = input.LastIndexOf(" ", length);
        return input.Substring(0, (iNextSpace > 0) ? iNextSpace : length).Trim() + ellipsisText;
    }

    public static string LineBreaksToBRs(this string input)
    {
        return input
            .Replace("\r\n", "<br>")
            .Replace("\n", "<br>")
            .Replace("\r", "<br>");
    }

    public static string TrimAndReplaceWhitespacesWithSingleSpace(this string stringToTrim)
    {
        return stringToTrim == null ? null : Regex.Replace(stringToTrim, @"\s+", " ").Trim();
    }

    public static string StripHTMLTags(this string input)
    {
        // Method should not be used to process untrusted user input; see http://stackoverflow.com/questions/4878452/remove-html-tags-in-string
        // for a safer version, see http://stackoverflow.com/questions/12787449/html-agility-pack-removing-unwanted-tags-without-removing-content
        const string htmlTagPattern = "<.*?>";
        return Regex.Replace(input, htmlTagPattern, " ");
    }
}