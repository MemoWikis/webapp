using System.Web.Mvc;

public class BetaController : BaseController
{
    public ActionResult Beta()
    {
        return Redirect("/");
    }

    public ActionResult Video1()
    {
        return View();
    }

    public JsonResult IsValidBetaUser(string betacode)
    {
        var isValidBetaCode = IsValidBetaCode.Yes(betacode);

        SessionUser.HasBetaAccess = isValidBetaCode;

        return new JsonResult{
            Data = new { IsValid = isValidBetaCode }
        };
    }

    public void SendBetaRequest(string email)
    {
        SendBetaRequestEmail.Run(email);
    }
}