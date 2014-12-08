using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class StringExtensions
{
    public static string Truncate(this string input, int maxLength)
    {
        if (string.IsNullOrEmpty(input))
            return input;
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
}