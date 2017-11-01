using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

public class RedirectController : Controller
{
    public ActionResult To(string googleCode)
    {
        return base.Redirect("https://goo.gl/" + googleCode);
    }

    public ActionResult ToHorseCertificate()
    {
        var category = Sl.R<CategoryRepository>().GetById(343);
        return RedirectPermanent(Links.CategoryDetail(category));
    }
}