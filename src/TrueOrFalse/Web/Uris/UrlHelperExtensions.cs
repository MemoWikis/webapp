using System.Web.Mvc;
using System.Web.Routing;

public static class UrlHelperExtensions
{
    public static string Action(this UrlHelper urlHelper, string actionName, string controllerName, object routeValues, bool ignoreCurrentRouteValues = false)
    {
        var routeValueDictionary = new RouteValueDictionary(routeValues);
        var requestContext = urlHelper.RequestContext;
        if (ignoreCurrentRouteValues)
        {
            var currentRouteData = requestContext.RouteData;
            var newRouteData = new RouteData(currentRouteData.Route, currentRouteData.RouteHandler);
            requestContext = new RequestContext(requestContext.HttpContext, newRouteData);
        }

        return UrlHelper.GenerateUrl(null, actionName, controllerName, routeValueDictionary,
            urlHelper.RouteCollection, requestContext, includeImplicitMvcValues: false);
    }
}