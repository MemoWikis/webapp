using System;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VueApp;

public class FacebookUsersController : BaseController
{
    private readonly VueSessionUser _vueSessionUser;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly RegisterUser _registerUser;
   
    private readonly JobQueueRepo _jobQueueRepo;

    public FacebookUsersController(VueSessionUser vueSessionUser,
        UserReadingRepo userReadingRepo,
        SessionUser sessionUser,
        RegisterUser registerUser,
        JobQueueRepo jobQueueRepo) : base(sessionUser)
    {
        _vueSessionUser = vueSessionUser;
        _userReadingRepo = userReadingRepo;
        _sessionUser = sessionUser;
        _registerUser = registerUser;
        _jobQueueRepo = jobQueueRepo;
    }

    public readonly record struct LoginJson(string facebookUserId, string facebookAccessToken);
    [HttpPost]
    public async Task<JsonResult> Login([FromBody] LoginJson json)
    {
        var user = _userReadingRepo.UserGetByFacebookId(json.facebookUserId);

        if (user == null)
        {
            return Json(new RequestResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.User.DoesNotExist
            });
        }

        if (await IsFacebookAccessToken.IsAccessTokenValidAsync(json.facebookAccessToken, json.facebookUserId))
        {
            _sessionUser.Login(user);

            return Json(new RequestResult
            {
                Success = true,
                Data = _vueSessionUser.GetCurrentUserData()
            });
        }

        return Json(new RequestResult
        {
            Success = false,
            MessageKey = FrontendMessageKeys.Error.User.InvalidFBToken
        });
    }

    public readonly record struct CreateAndLoginJson(FacebookUserCreateParameter facebookUser, string facebookAccessToken);

    [HttpPost]
    public async Task<JsonResult> CreateAndLogin([FromBody] CreateAndLoginJson json)
    {
        if (await IsFacebookAccessToken.IsAccessTokenValidAsync(json.facebookAccessToken, json.facebookUser.id))
        {
            var user = _userReadingRepo.UserGetByFacebookId(json.facebookUser.id);
            if (user != null)
            {
                _sessionUser.Login(user);
                return Json(new RequestResult
                {
                    Success = true,
                    Data = _vueSessionUser.GetCurrentUserData()
                });
            }

            var requestResult = _registerUser.SetFacebookUser(json.facebookUser);
            if (requestResult.Success)
            {
                return Json(new RequestResult
                {
                    Success = true,
                    Data = _vueSessionUser.GetCurrentUserData()
                });
            }

            return Json(requestResult);

        }

        return Json(new RequestResult
        {
            Success = false,
            MessageKey = FrontendMessageKeys.Error.User.InvalidFBToken
        });
    }
    
    [HttpGet]
    public JsonResult UserExists([FromBody] string facebookId)
    {
        return Json(_userReadingRepo.FacebookUserExists(facebookId));
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

        var mailMessage = new MailMessage(
            Settings.EmailFrom,
            Settings.EmailToMemucho,
            "Facebook Data Deletion Callback",
            $"The user with the Facebook Id {userData["user_id"]} has made a Facebook data deletion callback. Please delete the Account. Confirmation Code for this Ticket is {confirmationCode}.");

        SendEmail.Run(mailMessage, _jobQueueRepo, _userReadingRepo, MailMessagePriority.High);
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