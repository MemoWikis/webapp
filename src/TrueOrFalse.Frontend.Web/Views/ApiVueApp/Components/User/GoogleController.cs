using Google.Apis.Auth;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace VueApp;

public class GoogleController : BaseController
{
    private readonly VueSessionUser _vueSessionUser;
    private readonly RegisterUser _registerUser;
    private readonly CategoryRepository _categoryRepository;
    private readonly JobQueueRepo _jobQueueRepo;
    private readonly MessageRepo _messageRepo;
    private readonly UserReadingRepo _userReadingRepo;

    public GoogleController(SessionUser sessionUser,
        UserReadingRepo userReadingRepo,
        VueSessionUser vueSessionUser,
        RegisterUser registerUser,
        CategoryRepository categoryRepository,
        JobQueueRepo jobQueueRepo,
        MessageRepo messageRepo) : base(sessionUser)
    {
        _vueSessionUser = vueSessionUser;
        _registerUser = registerUser;
        _categoryRepository = categoryRepository;
        _jobQueueRepo = jobQueueRepo;
        _messageRepo = messageRepo;
        _userReadingRepo = userReadingRepo;
        _sessionUser = sessionUser;
    }

    public readonly record struct LoginJson(string token);
    [HttpPost]
    public async Task<JsonResult> Login([FromBody] LoginJson json)
    {
        var googleUser = await GetGoogleUser(json.token);
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
                return Json(CreateAndLogin(newUser));
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
    public RequestResult CreateAndLogin(GoogleUserCreateParameter googleUser)
    {
        var requestResult = _registerUser.SetGoogleUser(googleUser);
        if (requestResult.success)

        {
            return new RequestResult
            {
                success = true,
                data = _vueSessionUser.GetCurrentUserData()
            };
        }
        return requestResult;
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