using System.Web.Mvc;

[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
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