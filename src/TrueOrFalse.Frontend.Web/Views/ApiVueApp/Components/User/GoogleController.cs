using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VueApp;

public class GoogleController(
    SessionUser _sessionUser,
    UserReadingRepo _userReadingRepo,
    FrontEndUserData _frontEndUserData,
    RegisterUser _registerUser,
    IHttpContextAccessor _httpContextAccessor,
    PersistentLoginRepo _persistentLoginRepo) : Controller
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
                AppendGoogleCredentialCookie(json.token, _httpContextAccessor.HttpContext);

                return new LoginResult
                {
                    Success = result.Success,
                    MessageKey = result.MessageKey,
                    Data = _frontEndUserData.Get()
                };
            }

            _sessionUser.Login(user);
            AppendGoogleCredentialCookie(json.token, _httpContextAccessor.HttpContext);

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

    private void AppendGoogleCredentialCookie(string token, HttpContext httpContext)
    {
        RemovePersistentLoginFromCookie.Run(_persistentLoginRepo, _httpContextAccessor.HttpContext);
        httpContext.Response.Cookies.Append(PersistentLoginCookie.GoogleKey, token);
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
            Audience = new List<string> { Settings.GoogleClientId }
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