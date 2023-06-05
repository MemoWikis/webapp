using System;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TrueOrFalse.Frontend.Web.Code;

namespace VueApp;

public class FacebookUsersController : BaseController
{
    private readonly HttpContext _httpContext;

    public FacebookUsersController(HttpContext httpContext)
    {
        _httpContext = httpContext;
    }

    [HttpPost]
    public JsonResult Login(string facebookUserId, string facebookAccessToken)
    {
        var user = R<UserRepo>().UserGetByFacebookId(facebookUserId);

        if (user == null)
        {
            return Json(new
            {
                success = false,
                key = "userDoesntExist"
            });
        }

        if (IsFacebookAccessToken.Valid(facebookAccessToken, facebookUserId))
        {
            return Json(new
            {
                success = false,
                key = "invalidFBToken"
            });
        }

        SessionUserLegacy.Login(user);

        return Json(new
        {
            success = true,
            currentUser = new VueSessionUser(_httpContext).GetCurrentUserData()
        });
    }

    [HttpPost]
    public JsonResult CreateAndLogin(FacebookUserCreateParameter facebookUser)
    {
        var registerResult = RegisterUser.Run(facebookUser);
        if (registerResult.Success)
        {
            var user = Sl.UserRepo.UserGetByFacebookId(facebookUser.id);
            SendRegistrationEmail.Run(user);
            WelcomeMsg.Send(user);
            SessionUserLegacy.Login(user);
            var category = PersonalTopic.GetPersonalCategory(user);
            user.StartTopicId = category.Id;
            Sl.CategoryRepo.Create(category);
            SessionUserLegacy.User.StartTopicId = category.Id;

            return Json(new
            {
                Success = true,
                registerResult,
                localHref = Links.CategoryDetail(category.Name, category.Id),
                currentUser = new VueApp.VueSessionUser(_httpContext).GetCurrentUserData(),
            });
        }

        return Json(new
        {
            Success = false,
            registerResult
        });
    }
    
    [HttpPost]
    public JsonResult UserExists(string facebookId)
    {
        return Json(Sl.UserRepo.FacebookUserExists(facebookId));
    }

    [HttpPost]
    public string DataDeletionCallback(string jsonUserData)
    {
        JObject userData = JObject.Parse(jsonUserData);
        if(String.IsNullOrEmpty(GetHashString(userData["user_id"]?.ToString())))
        {
            return "Error";
        }
        var confirmationCode = GetHashString(userData["user_id"]?.ToString());

        SendEmail.Run(new MailMessage(
            Settings.EmailFrom,
            Settings.EmailToMemucho,
            "Facebook Data Deletion Callback",
            $"The user with the Facebook Id {userData["user_id"]} has made a Facebook data deletion callback. Please delete the Account. Confirmation Code for this Ticket is {confirmationCode}."), MailMessagePriority.High);
        var requestAnswer = new { url = "http://localhost:26590/FacebookUsersApi/UserExistsString?facebookId=" + userData["user_id"], confirmation_code = confirmationCode};
        return JsonConvert.SerializeObject(requestAnswer); ;
    }

    public static string GetHashString(string inputString)
    {
        StringBuilder sb = new StringBuilder();
        foreach (byte b in GetHash(inputString))
            sb.Append(b.ToString("X2"));

        return sb.ToString();
    }

    public static byte[] GetHash(string inputString)
    {
        using (HashAlgorithm algorithm = SHA256.Create())
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
    }

}