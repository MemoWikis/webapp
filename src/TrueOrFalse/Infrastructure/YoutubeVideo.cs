using System.Web;
using static System.String;

public class YoutubeVideo
{
    public static string GetVideoKeyFromUrl(string url)
    {
        if (IsNullOrEmpty(url) || !url.Contains("v="))
            return "";

        var parts = url.Split('?', '&');

        foreach (var part in parts)
            if (part.StartsWith("v="))
                return part.Substring(2);

        return "";
    }

    public static string GetIframe(string key) => 
        $@"<iframe width='100%' class='video' id='player' src='https://www.youtube.com/embed/{key}?enablejsapi=1&origin={HttpContext.Current.Request.Url.Scheme}://{HttpContext.Current.Request.Url.Host}'>
            </iframe> ";

    public static string GetPreviewImage(string youtubeKey)
        => $"https://img.youtube.com/vi/{youtubeKey}/0.jpg";

    public static string GetUrl(string youtubeKey)
        => $"https://www.youtube.com/watch?v={youtubeKey}";
}