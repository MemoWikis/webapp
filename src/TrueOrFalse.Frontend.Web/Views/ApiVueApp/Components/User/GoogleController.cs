using Google.Apis.Auth;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace VueApp;

public class GoogleController(
    SessionUser _sessionUser,
    UserReadingRepo _userReadingRepo,
    VueSessionUser _vueSessionUser,
    RegisterUser _registerUser) : Controller
{
    public readonly record struct LoginJson(string token);

    public readonly record struct LoginResult(bool Success, string MessageKey, VueSessionUser Data);

    [HttpPost]
    public async Task<LoginResult> Login([FromBody] LoginJson json)
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

                var result = CreateAndLogin(newUser);
                return new LoginResult
                {
                    Success = result.Success,
                    MessageKey = result.MessageKey
                };
            }

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
            MessageKey = FrontendMessageKeys.Error.Default
        };
    }

    [HttpPost]
    public bool UserExists(string googleId)
    {
        return _userReadingRepo.GoogleUserExists(googleId);
    }

    public readonly record struct CreateAndLoginResult(
        bool Success,
        VueSessionUser Data,
        string MessageKey);

    [HttpPost]
    public CreateAndLoginResult CreateAndLogin(GoogleUserCreateParameter googleUser)
    {
        var requestResult = _registerUser.SetGoogleUser(googleUser);
        if (requestResult.Success)

        {
            return new CreateAndLoginResult
            {
                Success = true,
                Data = _vueSessionUser.GetCurrentUserData()
            };
        }

        return new CreateAndLoginResult
        {
            Success = requestResult.Success,
            MessageKey = requestResult.MessageKey
        };
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
            Logg.r.Error(e.ToString());
            return null;
        }
    }
}