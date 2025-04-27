using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Razor;

/// <summary>
/// Copied from here: http://weblog.west-wind.com/posts/2012/May/30/Rendering-ASPNET-MVC-Views-to-String
/// 
/// Class that renders MVC views to a string using the
/// standard MVC View Engine to render the view. 
/// </summary>
public class ViewRenderer
{
    private readonly IRazorViewEngine _viewEngine;
    private readonly ITempDataProvider _tempDataProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ViewRenderer(IRazorViewEngine viewEngine, ITempDataProvider tempDataProvider, IServiceProvider serviceProvider, IHttpContextAccessor httpContextAccessor)
    {
        _viewEngine = viewEngine;
        _tempDataProvider = tempDataProvider;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Renders a partial MVC view to string. Use this method to render
    /// a partial view that doesn't merge with _Layout and doesn't fire
    /// _ViewStart.
    /// </summary>
    /// <param name="viewPath">
    /// The path to the view to render. Either in same controller, shared by 
    /// name or as fully qualified ~/ path including extension
    /// </param>
    /// <param name="model">The model to pass to the viewRenderer</param>
    /// <returns>String of the rendered view or null on error</returns>
    public string RenderPartialView(string viewPath, object model, ControllerContext controllerContext)
    {
        return RenderViewToStringInternal(viewPath, model, true);
    }

    /// <summary>
    /// Internal method that handles rendering of either partial or 
    /// or full views.
    /// </summary>
    /// <param name="viewPath">
    /// The path to the view to render. Either in same controller, shared by 
    /// name or as fully qualified ~/ path including extension
    /// </param>
    /// <param name="model">Model to render the view with</param>
    /// <param name="partial">Determines whether to render a full or partial view</param>
    /// <returns>String of the rendered view</returns>
    private string RenderViewToStringInternal(string viewPath, object model, bool partial = false)
    {
        var actionContext = new ActionContext(_httpContextAccessor.HttpContext, new Microsoft.AspNetCore.Routing.RouteData(), new ControllerActionDescriptor());

        using var sw = new StringWriter();
        var viewResult = partial ? _viewEngine.FindView(actionContext, viewPath, false) : _viewEngine.GetView(null, viewPath, false);

        if (viewResult.View == null)
        {
            throw new ArgumentNullException($"{viewPath} does not match any available view");
        }

        var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
        {
            Model = model
        };
        var viewContext = new ViewContext(
            actionContext,
            viewResult.View,
            viewDictionary,
            new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
            sw,
            new HtmlHelperOptions()
        );

        viewResult.View.RenderAsync(viewContext).Wait();
        return sw.ToString();
    }

}