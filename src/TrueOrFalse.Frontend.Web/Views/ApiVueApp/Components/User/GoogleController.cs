using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Oauth2.v2;
using Google.Apis.Services;
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
    public readonly record struct LoginJson(string? credential, string? accessToken);

    public readonly record struct LoginResult(
        bool Success,
        string MessageKey,
        FrontEndUserData.CurrentUserData Data);

    [HttpPost]
    public async Task<LoginResult> Login([FromBody] LoginJson json)
    {
        var (googleUser, token) = await GetGoogleUser(json.credential, json.accessToken);
        if (googleUser != null)
        {
            var user = _userReadingRepo.UserGetByGoogleId(googleUser.GoogleId);

            if (user == null)
            {
                var newUser = new GoogleUserCreateParameter
                {
                    Email = googleUser.Email,
                    UserName = googleUser.UserName,
                    GoogleId = googleUser.GoogleId
                };

                var result = _registerUser.CreateAndLogin(newUser);
                AppendGoogleCredentialCookie(token, _httpContextAccessor.HttpContext);

                return new LoginResult
                {
                    Success = result.Success,
                    MessageKey = result.MessageKey,
                    Data = _frontEndUserData.Get()
                };
            }

            _sessionUser.Login(user);
            AppendGoogleCredentialCookie(token, _httpContextAccessor.HttpContext);

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


    public class GoogleUser
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string GoogleId { get; set; }
    }

    public async Task<(GoogleUser, string)> GetGoogleUser(string? credential, string? accessToken)
    {
        if (credential != null)
        {
            var googleUser = await GetGoogleUserByCredential(credential);
            if (googleUser == null)
                return (null, credential);

            var newUser = new GoogleUser
            {
                Email = googleUser.Email,
                UserName = googleUser.Name,
                GoogleId = googleUser.Subject
            };
            return (newUser, credential);
        }
        if (accessToken != null)
        {
            var googleUser = await GetGoogleUserByAccessToken(accessToken);
            if (googleUser == null)
                return (null, accessToken);

            var newUser = new GoogleUser
            {
                Email = googleUser.Email,
                UserName = googleUser.Name,
                GoogleId = googleUser.Id
            };
            return (newUser, accessToken);
        }
        return (null, null);
    }

    public async Task<GoogleJsonWebSignature.Payload> GetGoogleUserByCredential(string credential)
    {
        var settings = new GoogleJsonWebSignature.ValidationSettings()
        {
            Audience = new List<string> { Settings.GoogleClientId }
        };

        try
        {
            return await GoogleJsonWebSignature.ValidateAsync(credential, settings);
        }
        catch (InvalidJwtException e)
        {
            Logg.r.Error(e.ToString());
            return null;
        }
    }

    public async Task<Google.Apis.Oauth2.v2.Data.Userinfo> GetGoogleUserByAccessToken(string accessToken)
    {
        var credential = GoogleCredential.FromAccessToken(accessToken);

        var oauth2Service = new Oauth2Service(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = "Memucho"
        });

        try
        {
            var userInfo = await oauth2Service.Userinfo.Get().ExecuteAsync();
            return userInfo;
        }
        catch (InvalidAccessException e)
        {
            Logg.r.Error(e.ToString());
            return null;
        }

    }
}