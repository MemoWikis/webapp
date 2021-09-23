using System;
using System.Web.Mvc;

public class GoogleUsersApiController : BaseController
{
    [HttpPost]
    public void Login(string googleId, string googleToken)
    {
        var user = R<UserRepo>().UserGetByGoogleId(googleId);

        if (user == null)
            throw new Exception($"google user {googleId} not found");

        if (!IsGoogleAccessToken.Valid(googleToken))
            throw new Exception("invalid google access token");

        R<SessionUser>().Login(user);
    }

    [HttpPost]
    public JsonResult UserExists(string googleId)
    {
        return Json(Sl.UserRepo.GoogleUserExists(googleId));
    }

    [HttpPost]
    public JsonResult CreateAndLogin(GoogleUserCreateParameter googleUser)
    {
        var registerResult = RegisterUser.Run(googleUser);

        if (registerResult.Success)
            R<SessionUser>().Login(
                Sl.UserRepo.UserGetByGoogleId(googleUser.GoogleId)
            );
        Sl.CategoryRepo.Create(PersonalTopic.GetPersonalCategory(Sl.UserRepo.UserGetByGoogleId(googleUser.GoogleId)));
        return new JsonResult { Data = registerResult };
    }

}