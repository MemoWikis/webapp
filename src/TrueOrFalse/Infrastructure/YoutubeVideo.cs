using System;
using static System.String;

public class YoutubeVideo
{
    public static string GetVideoKeyFromUrl(string url) =>
        !IsNullOrEmpty(url) && url.Contains("v=")
            ? url.Split(new[] {"v="}, StringSplitOptions.RemoveEmptyEntries)[1] 
            : "";
}