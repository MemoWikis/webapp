using System;
using System.Web;
using static System.String;

public class YoutubeVideo
{
    public static string GetVideoKeyFromUrl(string url) =>
        !IsNullOrEmpty(url) && url.Contains("v=")
            ? url.Split(new[] {"v="}, StringSplitOptions.RemoveEmptyEntries)[1] 
            : "";


    public static string GetIframe(string key) => 
        $@"<div class='youtubeContainer'>
               <iframe width='100%' class='video' id='player' src='https://www.youtube.com/embed/{key}?enablejsapi=1&origin={HttpContext.Current.Request.Url.Scheme}://{HttpContext.Current.Request.Url.Host}'>
               </iframe> 
           </div>";
}