using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;


//https://github.com/monperrus/crawler-user-agents/blob/master/crawler-user-agents.json file for actually crawlers
public class CrawlerRepo
{
    private static IList<Crawler>? _crawlers;
    private static readonly object _lockObj = new ();

    public static IList<Crawler>? GetAll(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
    {
        if (_crawlers != null)
            return _crawlers;

        lock (_lockObj)
        {
            InitCrawlers(httpContextAccessor, webHostEnvironment);

            return _crawlers;
        }
    }

    private static void InitCrawlers(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
    {
        var fileContent = File.ReadAllText(new PathTo(httpContextAccessor, webHostEnvironment).Crawlers());
        var crawlers = JsonConvert.DeserializeObject<IList<Crawler>>(fileContent);
        
        if (crawlers != null) 
            foreach (var crawler in crawlers) 
                crawler.Pattern = crawler.Pattern.ToLower();

        _crawlers = crawlers;
    }
}