using Microsoft.AspNetCore.Http;

public class IsCrawlerRequest
{
    public static bool Yes(HttpContext httpContext)
    {
        var userAgent = UserAgent.Get(httpContext);

        return CrawlerRepo.GetAll().Any(crawler => userAgent.Contains(crawler.Pattern));
    }
}