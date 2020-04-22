using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using NHibernate.Util;


//https://github.com/monperrus/crawler-user-agents/blob/master/crawler-user-agents.json file for actually crawlers
public class CrawlerRepo
{
    private static IList<Crawler> _crawlers;

    public static IList<Crawler> GetAll()
    {
        if (_crawlers != null)
            return _crawlers;

        lock ("A95EC747-38AB-45BA-9212-E52B9F47193C")
            InitCrawlers();

        return _crawlers;
    }

    private static void InitCrawlers()
    {
        var crawlers = JsonConvert.DeserializeObject<IList<Crawler>>(File.ReadAllText(PathTo.Crawlers()));
        
        if (crawlers != null) 
            foreach (var crawler in crawlers) 
                crawler.Pattern = crawler.Pattern.ToLower();

        _crawlers = crawlers;
    }
}