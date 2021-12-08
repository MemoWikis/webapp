using System;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        {
            var user = Sl.UserRepo.UserGetByFacebookId(facebookUser.id);
            R<SessionUser>().Login(user);
            var category = PersonalTopic.GetPersonalCategory(user);
            user.StartTopicId = category.Id;
            Sl.CategoryRepo.Create(category);
            _sessionUser.User.StartTopicId = category.Id;
        }

        return new JsonResult { Data = registerResult };
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

    [HttpPost]
    public string UserExistsString(string facebookId)
    {
        bool userExists = Sl.UserRepo.FacebookUserExists(facebookId);
        if (userExists)
        {
            return "Der Rückruf zur Datenlöschung für deinen Facebook Account wurde noch nicht ausgeführt. Bitte gib uns bis zu 48 Stunden Zeit deine Anfrage zu bearbeiten.";
        }

        return "Dieser Facebook Account ist nicht mit memucho verbunden";
    }
}