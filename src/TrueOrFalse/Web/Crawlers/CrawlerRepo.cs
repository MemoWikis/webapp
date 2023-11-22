using Newtonsoft.Json;

//https://github.com/monperrus/crawler-user-agents/blob/master/crawler-user-agents.json file for actually crawlers
public class CrawlerRepo
{
    private static IList<Crawler>? _crawlers;
    private static readonly object _lockObj = new();

    public static IList<Crawler> GetAll()
    {
        if (_crawlers != null)
            lock (_lockObj)
            {
                return _crawlers;
            }

        lock (_lockObj)
        {
            InitCrawlers();

            return _crawlers!;
        }
    }

    private static void InitCrawlers()
    {
        var fileContent = File.ReadAllText(PathTo.Crawlers());
        var crawlers = JsonConvert.DeserializeObject<IList<Crawler>>(fileContent);
        
        if (crawlers != null) 
            foreach (var crawler in crawlers) 
                crawler.Pattern = crawler.Pattern.ToLower();

        _crawlers = crawlers;
    }
}