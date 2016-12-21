using System.Web.Mvc;

public class UsersApiController : BaseController
{
    public JsonResult FacebookUserExists(string facebookId)
    {
        return Json(R<UserRepo>().FacebookUserExists(facebookId));
    }

    public void CreateAndLogin(FacebookUserCreateParameter facebookUserCreateParameter)
    {
        R<RegisterUser>().Run(facebookUserCreateParameter);
    }

    public void Login(string facebookId)
    {
    }
}