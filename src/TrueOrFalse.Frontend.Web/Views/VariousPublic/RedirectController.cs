using System.Web.Mvc;

public class RedirectController : Controller
{
    public ActionResult To(string googleCode)
    {
        return base.Redirect("https://goo.gl/" + googleCode);
    }
}