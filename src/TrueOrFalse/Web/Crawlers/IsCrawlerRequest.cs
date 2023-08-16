using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

public class IsCrawlerRequest
{
    public static bool Yes(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
    {
        var userAgent = httpContextAccessor.HttpContext.Request.Headers["User-Agent"];
        if (CrawlerRepo.GetAll(httpContextAccessor, webHostEnvironment)
            .Any(crawler => userAgent.Contains(crawler.Pattern)))
            return true;

        return false;
    }
}