using System;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TrueOrFalse.Frontend.Web.Code;

namespace VueApp;

public class FacebookUsersController : Controller
{
    private readonly VueSessionUser _vueSessionUser;
    private readonly UserRepo _userRepo;
    private readonly SessionUser _sessionUser;
    private readonly RegisterUser _registerUser;
    private readonly CategoryRepository _categoryRepository;

    public FacebookUsersController(VueSessionUser vueSessionUser,
        UserRepo userRepo,
        SessionUser sessionUser,RegisterUser registerUser,
        CategoryRepository categoryRepository)
    {
        _vueSessionUser = vueSessionUser;
        _userRepo = userRepo;
        _sessionUser = sessionUser;
        _registerUser = registerUser;
        _categoryRepository = categoryRepository;
    }

    [HttpPost]
    public async Task<JsonResult> Login(string facebookUserId, string facebookAccessToken)
    {
        var user = _userRepo.UserGetByFacebookId(facebookUserId);

        if (user == null)
        {
            return Json(new RequestResult
            {
                success = false,
                messageKey = FrontendMessageKeys.Error.User.DoesNotExist
            });
        }

        if (await IsFacebookAccessToken.IsAccessTokenValidAsync(facebookAccessToken, facebookUserId))
        {
            _sessionUser.Login(user);

            return Json(new RequestResult
            {
                success = true,
                data = _vueSessionUser.GetCurrentUserData()
            });
        }

        return Json(new RequestResult
        {
            success = false,
            messageKey = FrontendMessageKeys.Error.User.InvalidFBToken
        });
    }

    [HttpPost]
    public async Task<JsonResult> CreateAndLogin(FacebookUserCreateParameter facebookUser, string facebookAccessToken)
    {
        if (await IsFacebookAccessToken.IsAccessTokenValidAsync(facebookAccessToken, facebookUser.id))
        {
            var user = _userRepo.UserGetByFacebookId(facebookUser.id);
            if (user != null)
            {
                _sessionUser.Login(user);

                return Json(new RequestResult
                {
                    success = true,
                    data = _vueSessionUser.GetCurrentUserData()
                });
            }
            var registerResult = _registerUser.Run(facebookUser);

            if (registerResult.Success)
            {
                var newUser = _userRepo.UserGetByFacebookId(facebookUser.id);
                _registerUser.CreateStartTopicAndSetToUser(user);
                _registerUser.SendWelcomeAndRegistrationEmails(user);

                _sessionUser.Login(newUser);

                return Json(new RequestResult
                {
                    success = true,
                    data = _vueSessionUser.GetCurrentUserData(),
                });
            }
            if (registerResult.EmailAlreadyInUse)
            {
                return Json(new RequestResult
                {
                    success = false,
                    messageKey = FrontendMessageKeys.Error.User.EmailInUse
                });
            }
        }

        return Json(new RequestResult
        {
            success = false,
            messageKey = FrontendMessageKeys.Error.User.InvalidFBToken
        });
    }
    
    [HttpGet]
    public JsonResult UserExists(string facebookId)
    {
        return Json(_userRepo.FacebookUserExists(facebookId), JsonRequestBehavior.AllowGet);
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