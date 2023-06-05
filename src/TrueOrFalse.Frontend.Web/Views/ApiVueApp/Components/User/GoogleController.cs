using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace VueApp;

public class GoogleController : BaseController
{
    private readonly HttpContext _httpContext;

    public GoogleController(HttpContext httpContext)
    {
        _httpContext = httpContext;
    }

    [HttpPost]
    public async Task<JsonResult> Login(string token)
    {
        var googleUser = await GetGoogleUser(token);
        if (googleUser != null)
        {
            var user = R<UserRepo>().UserGetByGoogleId(googleUser.Subject);

            if (user == null)
            {
                var newUser = new GoogleUserCreateParameter
                {
                    Email = googleUser.Email,
                    UserName = googleUser.Name,
                    GoogleId = googleUser.Subject
                };
                return CreateAndLogin(newUser);
            }

            SessionUserLegacy.Login(user);
            return Json(new
            {
                success = true,
                currentUser = new VueSessionUser(_httpContext).GetCurrentUserData()
            });
        }

        return Json(new
        {
            success = false,
        });
       
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
            SendRegistrationEmail.Run(user);
            WelcomeMsg.Send(user);
            SessionUserLegacy.Login(user);
            var category = PersonalTopic.GetPersonalCategory(user);
            user.StartTopicId = category.Id;
            Sl.CategoryRepo.Create(category);
            SessionUserLegacy.User.StartTopicId = category.Id;
        }

        return Json(new
        {
            success = true,
            CurrentUser = new VueSessionUser(_httpContext).GetCurrentUserData()
        });
    }

    public async Task<GoogleJsonWebSignature.Payload> GetGoogleUser(string token)
    {
        var settings = new GoogleJsonWebSignature.ValidationSettings()
        {
            Audience = new List<string>() { "290065015753-gftdec8p1rl8v6ojlk4kr13l4ldpabc8.apps.googleusercontent.com" }
        };

        try
        {
            return await GoogleJsonWebSignature.ValidateAsync(token, settings);
        }
        catch (InvalidJwtException e)
        {
            return null;
        }
    }
}