using System;
using static System.String;

public class YoutubeVideo
{
    public static string GetVideoKeyFromUrl(string url) =>
        !IsNullOrEmpty(url) && url.Contains("v=")
            ? url.Split(new[] {"v="}, StringSplitOptions.RemoveEmptyEntries)[1] 
            : "";


    public static string GetIframe(string key) => 
        $@"<div class='youtubeContainer'>
               <iframe width='100%' class='video' src='https://www.youtube.com/embed/{key}'>
               </iframe> 
           </div>";
}