using static System.String;

public class SeoUtils
{
    public static string ReplaceDoubleQuotes(string value) => 
        IsNullOrEmpty(value) 
            ? "" 
            : value.Replace("\"", "'").Replace("„", "'").Replace("“", "'");
}

