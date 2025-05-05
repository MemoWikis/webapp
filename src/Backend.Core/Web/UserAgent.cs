using Microsoft.AspNetCore.Http;

public class UserAgent
{
    public static string Get(HttpContext httpContext)
    {
        return httpContext
            .Request
            .Headers["User-Agent"]
            .ToString()
            .ToLower();
    }
}