using System.Web.Mvc;

public class SharedController : Controller
{
    public string RenderActivityPopupContent()
    {
        return ViewRenderer.RenderPartialView(
            "~/Views/Shared/ActivityPopupContent.ascx",
            null, 
            ControllerContext
        );
    }
}