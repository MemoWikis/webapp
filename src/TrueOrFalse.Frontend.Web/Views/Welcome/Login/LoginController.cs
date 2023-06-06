using System.Web.Mvc;

public class MVCLoginController : BaseController
{
    public MVCLoginController(SessionUser sessionUser) : base(sessionUser)
    {
        
    }

    [HttpPost]
    public JsonResult IsUserNameAvailable(string selectedName) =>
        new JsonResult { Data = new { isAvailable = global::IsUserNameAvailable.Yes(selectedName) } };

    [HttpPost]
    public JsonResult IsEmailAvailable(string selectedEmail) =>
        new JsonResult { Data = new { isAvailable = IsEmailAddressAvailable.Yes(selectedEmail) } };

}