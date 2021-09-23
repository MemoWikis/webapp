using System;
using System.Web.Mvc;

public class FacebookUsersApiController : BaseController
{
    [HttpPost]
    public void Login(string facebookUserId, string facebookAccessToken)
    {
        var user = R<UserRepo>().UserGetByFacebookId(facebookUserId);

        if (user == null)
            throw new Exception($"facebook user {facebookUserId} not found");

        if (IsFacebookAccessToken.Valid(facebookAccessToken, facebookUserId))
            throw new Exception("invalid facebook access token");

        R<SessionUser>().Login(user);
    }

    [HttpPost]
    public JsonResult CreateAndLogin(FacebookUserCreateParameter facebookUser)
    {
        var registerResult = RegisterUser.Run(facebookUser);

        if (registerResult.Success)
            R<SessionUser>().Login(
                Sl.UserRepo.UserGetByFacebookId(facebookUser.id)
            );

        Sl.CategoryRepo.Create(PersonalTopic.GetPersonalCategory(Sl.UserRepo.UserGetByFacebookId(facebookUser.id)));
        return new JsonResult { Data = registerResult };
    }
    
    [HttpPost]
    public JsonResult UserExists(string facebookId)
    {
        return Json(Sl.UserRepo.FacebookUserExists(facebookId));
    }
}