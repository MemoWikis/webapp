using System.Text.RegularExpressions;

public static class SafeQuestionTitle
{
    /// <summary>
    /// Generates a safe text version of a question title by removing figure elements and HTML tags
    /// </summary>
    /// <param name="htmlText">The HTML text to process</param>
    /// <returns>Clean text suitable for question titles</returns>
    public static string Get(string htmlText)
    {
        if (string.IsNullOrEmpty(htmlText))
            return string.Empty;
            
        // First, remove figure elements with tiptap-figure class and their entire content
        var figurePattern = @"<figure[^>]*class=""[^""]*tiptap-figure[^""]*""[^>]*>.*?</figure>";
        var textWithoutFigures = Regex.Replace(htmlText, figurePattern, "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
        
        // Replace block-level and line-breaking HTML tags with spaces to maintain word separation
        var blockTags = @"</?(p|div|br|h[1-6]|li|ul|ol|blockquote|pre)[^>]*>";
        var textWithSpaces = Regex.Replace(textWithoutFigures, blockTags, " ", RegexOptions.IgnoreCase);
        
        // Remove all remaining HTML tags
        var textWithoutTags = Regex.Replace(textWithSpaces, "<.*?>", "");
        
        // Clean up multiple spaces and trim
        var cleanText = Regex.Replace(textWithoutTags, @"\s+", " ").Trim();
        
        return cleanText;
    }
}
