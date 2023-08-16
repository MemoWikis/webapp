using System.Web;
using Microsoft.AspNetCore.Http;


public class UserAgent
{
    public static string Get(IHttpContextAccessor httpContextAccessor)
    {
        return httpContextAccessor
            .HttpContext?.
            Request.
            Headers["User-Agent"].
            ToString().
            ToLower() ?? "";
    }
}