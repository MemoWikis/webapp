using System.Linq;
using System.Web;

public class IsCrawlerRequest
{
    public static bool Yes(string userAgent)
    {
        if (HttpContext.Current.Request.Browser.Crawler)
            return true;

        if (CrawlerRepo.GetAll().Any(crawler => userAgent.Contains(crawler.Pattern)))
            return true;

        return false;
    }
}