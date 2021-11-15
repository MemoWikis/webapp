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
        {
            var user = Sl.UserRepo.UserGetByGoogleId(googleUser.GoogleId); 
            R<SessionUser>().Login(user);
            var category = PersonalTopic.GetPersonalCategory(user);
            user.StartTopicId = category.Id;
            Sl.CategoryRepo.Create(category);
            _sessionUser.User.StartTopicId = category.Id;
        }

        return new JsonResult { Data = registerResult };
    }

}