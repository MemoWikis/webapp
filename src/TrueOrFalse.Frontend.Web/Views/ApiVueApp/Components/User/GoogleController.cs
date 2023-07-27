using Google.Apis.Auth;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace VueApp;

public class GoogleController : Controller
{
    private readonly VueSessionUser _vueSessionUser;
    private readonly RegisterUser _registerUser;
    private readonly CategoryRepository _categoryRepository;
    private readonly JobQueueRepo _jobQueueRepo;
    private readonly MessageRepo _messageRepo;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly SessionUser _sessionUser;

    public GoogleController(SessionUser sessionUser,
        UserReadingRepo userReadingRepo,
        VueSessionUser vueSessionUser,
        RegisterUser registerUser,
        CategoryRepository categoryRepository,
        JobQueueRepo jobQueueRepo,
        MessageRepo messageRepo)
    {
        _vueSessionUser = vueSessionUser;
        _registerUser = registerUser;
        _categoryRepository = categoryRepository;
        _jobQueueRepo = jobQueueRepo;
        _messageRepo = messageRepo;
        _userReadingRepo = userReadingRepo;
        _sessionUser = sessionUser;
    }

    [HttpPost]
    public async Task<JsonResult> Login(string token)
    {
        var googleUser = await GetGoogleUser(token);
        if (googleUser != null)
        {
            var user = _userReadingRepo.UserGetByGoogleId(googleUser.Subject);

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
            messageKey = FrontendMessageKeys.Error.Default
        });

    }

    [HttpPost]
    public JsonResult UserExists(string googleId)
    {
        return Json(_userReadingRepo.GoogleUserExists(googleId));
    }

    [HttpPost]
    public JsonResult CreateAndLogin(GoogleUserCreateParameter googleUser)
    {
        var registerResult = _registerUser.Run(googleUser);

        if (registerResult.Success)
        {
            var user = _userReadingRepo.UserGetByGoogleId(googleUser.GoogleId);
            SendRegistrationEmail.Run(user, _jobQueueRepo, _userReadingRepo);
            WelcomeMsg.Send(user, _messageRepo);
            _sessionUser.Login(user);
            var category = PersonalTopic.GetPersonalCategory(user);
            _categoryRepository.Create(category);
            user.StartTopicId = category.Id;
            _sessionUser.User.StartTopicId = category.Id;
        }
        else
        {
            return Json(new RequestResult
            {
                success = false,
                messageKey = FrontendMessageKeys.Error.User.EmailInUse
            });
        }

        return Json(new RequestResult
        {
            success = true,
            data = _vueSessionUser.GetCurrentUserData()
        });
    }


    public async Task<GoogleJsonWebSignature.Payload> GetGoogleUser(string token)
    {
        var settings = new GoogleJsonWebSignature.ValidationSettings()
        {
            Audience = new List<string>()
                    { "290065015753-gftdec8p1rl8v6ojlk4kr13l4ldpabc8.apps.googleusercontent.com" }
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
