using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;


namespace TrueOrFalse;

public class RouteConfig
{
    public static void RegisterRoutes(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("{resource}.axd/{*pathInfo}", (string resource, string pathInfo) => Results.NotFound())
            .ExcludeFromDescription();

        endpoints.MapGet("{favicon:regex((.*/)?favicon.ico(/.*)?)}", (string favicon) => Results.NotFound())
            .ExcludeFromDescription();
    }
}