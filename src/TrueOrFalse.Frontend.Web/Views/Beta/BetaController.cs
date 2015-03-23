using System.Web.Mvc;

public class BetaController : Controller
{
    public ActionResult Beta()
    {
        return View();
    }

    public JsonResult IsValidBetaUser(string betacode)
    {
        return new JsonResult
        {
            Data = new
            {
                IsValid = IsValidBetaCode.Yes(betacode)
            }
        };
    }
}