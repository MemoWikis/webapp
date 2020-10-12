using System.IO;
using System.Web.Mvc;

/// <summary>
/// Copied from here: http://weblog.west-wind.com/posts/2012/May/30/Rendering-ASPNET-MVC-Views-to-String
/// 
/// Class that renders MVC views to a string using the
/// standard MVC View Engine to render the view. 
/// </summary>
public class ViewRenderer
{
    /// <summary>
    /// Renders a full MVC view to a string. Will render with the full MVC
    /// View engine including running _ViewStart and merging into _Layout        
    /// </summary>
    /// <param name="viewPath">
    /// The path to the view to render. Either in same controller, shared by 
    /// name or as fully qualified ~/ path including extension
    /// </param>
    /// <param name="model">The model to render the view with</param>
    /// <returns>String of the rendered view or null on error</returns>
    public static string RenderView(string viewPath, object model, ControllerContext controllerContext)
    {
        return RenderViewToStringInternal(viewPath, model, controllerContext, false);
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
    public static string RenderPartialView(string viewPath, object model, ControllerContext controllerContext)
    {
        return RenderViewToStringInternal(viewPath, model, controllerContext, true);
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
    protected static string RenderViewToStringInternal(string viewPath, object model,
        ControllerContext controllerContext, bool partial = false)
    {
        // first find the ViewEngine for this view
        ViewEngineResult viewEngineResult;
        if (partial)
            viewEngineResult = ViewEngines.Engines.FindPartialView(controllerContext, viewPath);
        else
            viewEngineResult = ViewEngines.Engines.FindView(controllerContext, viewPath, null);

        if (viewEngineResult == null)
            throw new FileNotFoundException();

        // get the view and attach the model to view data
        var view = viewEngineResult.View;
        controllerContext.Controller.ViewData.Model = model;

        string result;

        using (var sw = new StringWriter())
        {
            var ctx = new ViewContext(controllerContext, view,
                controllerContext.Controller.ViewData,
                controllerContext.Controller.TempData,
                sw);
            view.Render(ctx, sw);
            result = sw.ToString();
        }

        return result;
    }

}