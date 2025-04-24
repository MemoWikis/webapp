using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

public class AccessOnlyLocalAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        var request = filterContext.HttpContext.Request;

        if (!request.IsLocal())
            throw new InvalidAccessException();

        base.OnActionExecuting(filterContext);  
    }
}

public static class HttpRequestExtensions
{
    public static bool IsLocal(this HttpRequest request)
    {
        var connection = request.HttpContext.Connection;
        if (connection.RemoteIpAddress != null)
        {
            return connection.LocalIpAddress != null && connection.RemoteIpAddress.Equals(connection.LocalIpAddress) || IPAddress.IsLoopback(connection.RemoteIpAddress);
        }
        return true;
    }
}