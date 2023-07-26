using System;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TrueOrFalse.Frontend.Web.Code;

namespace VueApp;

public class FacebookUsersController : Controller
{
    private readonly VueSessionUser _vueSessionUser;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly SessionUser _sessionUser;
    private readonly RegisterUser _registerUser;
    private readonly CategoryRepository _categoryRepository;
    private readonly JobQueueRepo _jobQueueRepo;
    private readonly MessageRepo _messageRepo;

    public FacebookUsersController(VueSessionUser vueSessionUser,
        UserReadingRepo userReadingRepo,
        SessionUser sessionUser,
        RegisterUser registerUser,
        CategoryRepository categoryRepository,
        JobQueueRepo jobQueueRepo,
        MessageRepo messageRepo)
    {
        _vueSessionUser = vueSessionUser;
        _userReadingRepo = userReadingRepo;
        _sessionUser = sessionUser;
        _registerUser = registerUser;
        _categoryRepository = categoryRepository;
        _jobQueueRepo = jobQueueRepo;
        _messageRepo = messageRepo;
    }

    [HttpPost]
    public JsonResult Login(string facebookUserId, string facebookAccessToken)
    {
        var user = _userReadingRepo.UserGetByFacebookId(facebookUserId);

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

        _sessionUser.Login(user);

        return Json(new
        {
            success = true,
            currentUser = _vueSessionUser.GetCurrentUserData()
        });
    }

    [HttpPost]
    public JsonResult CreateAndLogin(FacebookUserCreateParameter facebookUser)
    {
        var registerResult = _registerUser.Run(facebookUser);
        if (registerResult.Success)
        {
            var user = _userReadingRepo.UserGetByFacebookId(facebookUser.id);
            SendRegistrationEmail.Run(user, _jobQueueRepo, _userReadingRepo);
            WelcomeMsg.Send(user, _messageRepo);
            _sessionUser.Login(user);
            var category = PersonalTopic.GetPersonalCategory(user);
            user.StartTopicId = category.Id;
            _categoryRepository.Create(category);
            _sessionUser.User.StartTopicId = category.Id;

            return Json(new
            {
                Success = true,
                registerResult,
                localHref = Links.CategoryDetail(category.Name, category.Id),
                currentUser = _vueSessionUser.GetCurrentUserData(),
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