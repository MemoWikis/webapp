using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Newtonsoft.Json;

public class BlogMemuchoDeRepo
{

    private const string _baseBlogApiUrl = "http://blog.memucho.de/wp-json/wp/v2/posts?_embed"; //http://blog.memucho.de/wp-json/wp/v2/posts?_embed&per_page=1

    public static IList<WordpressBlogPost> GetRecentPosts(int amount)
    {
        try
        {
            var url = _baseBlogApiUrl;
            url = url + "&per_page=" + amount;
            
            var response = new MyWebClient()
                .DownloadString(url);

            return JsonConvert.DeserializeObject<WordpressBlogPost[]>(response).ToList();

        }
        catch (Exception e)
        {
            Logg.Error(e);
            return new List<WordpressBlogPost>();
        }
    }
}

public class MyWebClient : WebClient
{
    protected override WebRequest GetWebRequest(Uri uri)
    {
        WebRequest w = base.GetWebRequest(uri);
        w.Timeout = 1000;
        return w;
    }
}