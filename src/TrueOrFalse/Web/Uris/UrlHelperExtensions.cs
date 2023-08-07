﻿using Azure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;

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

    public static void RemoveRoutes(this UrlHelper urlHelper, string[] routesToRemove)
    {
        RouteValueDictionary currentRouteData = urlHelper.RequestContext.RouteData.Values;
        if (routesToRemove != null && routesToRemove.Length > 0)
        {
            foreach (string route in routesToRemove)
            {
                currentRouteData.Remove(route);
            }
        }
    }
}