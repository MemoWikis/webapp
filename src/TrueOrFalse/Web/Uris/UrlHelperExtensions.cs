using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;

public static class UrlHelperExtensions
{
    public static string Action(
        this IUrlHelper urlHelper,
        string actionName,
        string controllerName,
        object routeValues,
        bool ignoreCurrentRouteValues = false)
    {
        if (ignoreCurrentRouteValues)
        {
            return urlHelper.Action(actionName, controllerName, routeValues);
        }

        var currentRouteValues = urlHelper.ActionContext.RouteData.Values;
        var mergedRouteValues = new RouteValueDictionary(routeValues);
        foreach (var pair in currentRouteValues)
        {
            if (!mergedRouteValues.ContainsKey(pair.Key))
            {
                mergedRouteValues[pair.Key] = pair.Value;
            }
        }

        return urlHelper.Action(actionName, controllerName, mergedRouteValues);
    }

    public static void RemoveRoutes(this IUrlHelper urlHelper, string[] routesToRemove)
    {
        RouteValueDictionary currentRouteData = urlHelper.ActionContext.RouteData.Values;
        if (routesToRemove != null && routesToRemove.Length > 0)
        {
            foreach (string route in routesToRemove)
            {
                currentRouteData.Remove(route);
            }
        }
    }
}