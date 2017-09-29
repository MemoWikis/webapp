using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;

public class BlogMemuchoDeRepo
{
    private const string _baseBlogApiUrl = "http://blog.memucho.de/wp-json/wp/v2/posts?_embed"; //http://blog.memucho.de/wp-json/wp/v2/posts?_embed&per_page=1

    public static IList<WordpressBlogPost> GetRecentPosts(int amount)
    {
        var url = _baseBlogApiUrl;
        url = url + "&per_page=" + amount;

        var response = new WebClient()
            .DownloadString(url);

        return JsonConvert.DeserializeObject<WordpressBlogPost[]>(response).ToList();
    }

}
