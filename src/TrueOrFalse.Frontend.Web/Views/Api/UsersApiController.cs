using System;
using System.Web.Mvc;

public class UsersApiController : BaseController
{
    [HttpPost]
    public JsonResult FacebookUserExists(string facebookId)
    {
        return Json(R<UserRepo>().FacebookUserExists(facebookId));
    }

    [HttpPost]
    public JsonResult GoogleUserExists(string googleId)
    {
        return Json(R<UserRepo>().GoogleUserExists(googleId));
    }

    [HttpGet]
    public EmptyResult CreateAndLogin()
    {
        return new EmptyResult();
    }

    [HttpPost]
    public JsonResult CreateAndLogin(FacebookUserCreateParameter facebookUserCreateParameter)
    {
        return new JsonResult {Data = R<RegisterUser>().Run(facebookUserCreateParameter) };
    }

    [HttpPost]
    public void Login(string facebookUserId, string facebookAccessToken)
    {
        var user = R<UserRepo>().UserGetByFacebookId(facebookUserId);

        if(user == null)
            throw new Exception($"facebook user {facebookUserId} not found");

        if(IsFacebookAccessToken.Valid(facebookAccessToken, facebookUserId))
            throw new Exception("invalid facebook access token");

        R<SessionUser>().Login(user);
    }
}