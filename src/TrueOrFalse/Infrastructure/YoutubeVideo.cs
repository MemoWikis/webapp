using System.Text.RegularExpressions;
using System.Web;
using Microsoft.AspNetCore.Http;

public class YoutubeVideo
{
    private const string YoutubeLinkRegex = "^((?:https?:)?\\/\\/)?((?:www|m)\\.)?((?:youtube\\.com|youtu.be))(\\/(?:[\\w\\-]+\\?v=|embed\\/|v\\/)?)([\\w\\-]+)(\\S+)?$";
    private static readonly Regex regexExtractId = new Regex(YoutubeLinkRegex, RegexOptions.Compiled);
    public static string GetVideoKeyFromUrl(string url)
    {
      //extract the id
        var regRes = regexExtractId.Match(url);
        if (regRes.Success)
        {
            return regRes.Groups[5].Value;
        }
        return null;
    }

    public static string GetPreviewImage(string youtubeKey)
        => $"https://img.youtube.com/vi/{youtubeKey}/0.jpg";

    public static string GetUrl(string youtubeKey)
        => $"https://www.youtube.com/watch?v={youtubeKey}";
}