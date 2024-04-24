using System;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VueApp;

public class FacebookUsersController(
    VueSessionUser _vueSessionUser,
    UserReadingRepo _userReadingRepo,
    SessionUser _sessionUser,
    RegisterUser _registerUser,
    JobQueueRepo _jobQueueRepo) : Controller
{
    public readonly record struct LoginJson(string facebookUserId, string facebookAccessToken);

    public readonly record struct LoginResult(
        bool Success,
        string MessageKey,
        VueSessionUser.CurrentUserData Data);

    [HttpPost]
    public async Task<LoginResult> Login([FromBody] LoginJson json)
    {
        var user = _userReadingRepo.UserGetByFacebookId(json.facebookUserId);

        if (user == null)
        {
            return new LoginResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.User.DoesNotExist
            };
        }

        if (await IsFacebookAccessToken.IsAccessTokenValidAsync(json.facebookAccessToken,
                json.facebookUserId))
        {
            _sessionUser.Login(user);

            return new LoginResult
            {
                Success = true,
                Data = _vueSessionUser.GetCurrentUserData()
            };
        }

        return new LoginResult
        {
            Success = false,
            MessageKey = FrontendMessageKeys.Error.User.InvalidFBToken
        };
    }

    public readonly record struct CreateAndLoginJson(
        FacebookUserCreateParameter facebookUser,
        string facebookAccessToken);

    [HttpPost]
    public async Task<LoginResult> CreateAndLogin([FromBody] CreateAndLoginJson json)
    {
        if (await IsFacebookAccessToken.IsAccessTokenValidAsync(json.facebookAccessToken,
                json.facebookUser.id))
        {
            var user = _userReadingRepo.UserGetByFacebookId(json.facebookUser.id);
            if (user != null)
            {
                _sessionUser.Login(user);
                return new LoginResult
                {
                    Success = true,
                    Data = _vueSessionUser.GetCurrentUserData()
                };
            }

            var registerResult = _registerUser.SetFacebookUser(json.facebookUser);
            if (registerResult.Success)
            {
                return new LoginResult
                {
                    Success = true,
                    Data = _vueSessionUser.GetCurrentUserData()
                };
            }

            return new LoginResult
            {
                Success = registerResult.Success,
                MessageKey = registerResult.MessageKey
            };
        }

        return new LoginResult
        {
            Success = false,
            MessageKey = FrontendMessageKeys.Error.User.InvalidFBToken
        };
    }

    [HttpGet]
    public bool UserExists([FromBody] string facebookId)
    {
        return _userReadingRepo.FacebookUserExists(facebookId);
    }

    [HttpPost]
    public string DataDeletionCallback(string jsonUserData)
    {
        JObject userData = JObject.Parse(jsonUserData);
        if (String.IsNullOrEmpty(GetHashString(userData["user_id"]?.ToString())))
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
        var requestAnswer = new
        {
            url = "http://localhost:26590/FacebookUsersApi/UserExistsString?facebookId=" +
                  userData["user_id"],
            confirmation_code = confirmationCode
        };
        return JsonConvert.SerializeObject(requestAnswer);
        ;
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