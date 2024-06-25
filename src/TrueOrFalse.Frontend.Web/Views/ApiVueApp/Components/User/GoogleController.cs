using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VueApp;

public class GoogleController(
    SessionUser _sessionUser,
    UserReadingRepo _userReadingRepo,
    FrontEndUserData _frontEndUserData,
    RegisterUser _registerUser) : Controller
{
    public readonly record struct LoginJson(string token);

    public readonly record struct LoginResult(
        bool Success,
        string MessageKey,
        FrontEndUserData.CurrentUserData Data);

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

                var result = _registerUser.CreateAndLogin(newUser);

                return new LoginResult
                {
                    Success = result.Success,
                    MessageKey = result.MessageKey,
                    Data = _frontEndUserData.Get()
                };
            }

            _sessionUser.Login(user);
            return new LoginResult
            {
                Success = true,
                Data = _frontEndUserData.Get()
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