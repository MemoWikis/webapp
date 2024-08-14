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
    public readonly record struct LoginRequest(string? credential, string? accessToken);

    public readonly record struct LoginResponse(
        bool Success,
        string MessageKey,
        FrontEndUserData.CurrentUserData Data);

    [HttpPost]
    public async Task<LoginResponse> Login([FromBody] LoginRequest request)
    {
        var googleUser = await GetGoogleUser(request.credential, request.accessToken);
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

                AppendGoogleCredentialCookie(_httpContextAccessor.HttpContext, request.credential, request.accessToken);

                return new LoginResponse
                {
                    Success = result.Success,
                    MessageKey = result.MessageKey,
                    Data = _frontEndUserData.Get()
                };
            }

            _sessionUser.Login(user);
            AppendGoogleCredentialCookie(_httpContextAccessor.HttpContext, request.credential, request.accessToken);

            return new LoginResponse
            {
                Success = true,
                Data = _frontEndUserData.Get()
            };
        }

        return new LoginResponse
        {
            Success = false,
            MessageKey = FrontendMessageKeys.Error.Default
        };
    }

    private void AppendGoogleCredentialCookie(HttpContext httpContext, string? credential = null, string? accessToken = null)
    {
        if (_httpContextAccessor.HttpContext == null)
            return;

        RemovePersistentLoginFromCookie.Run(_persistentLoginRepo, _httpContextAccessor.HttpContext);
        if (credential != null) httpContext.Response.Cookies.Append(PersistentLoginCookie.GoogleCredential, credential);
        else if (accessToken != null) httpContext.Response.Cookies.Append(PersistentLoginCookie.GoogleAccessToken, accessToken);
    }

    public class GoogleUser
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string GoogleId { get; set; }
    }

    private async Task<GoogleUser> GetGoogleUser(string? credential, string? accessToken)
    {
        if (credential != null)
        {
            var googleUser = await GetGoogleUserByCredential(credential);
            if (googleUser == null)
                return null;

            var newUser = new GoogleUser
            {
                Email = googleUser.Email,
                UserName = googleUser.Name,
                GoogleId = googleUser.Subject
            };
            return newUser;
        }
        if (accessToken != null)
        {
            var googleUser = await GetGoogleUserByAccessToken(accessToken);
            if (googleUser == null)
                return null;

            var newUser = new GoogleUser
            {
                Email = googleUser.Email,
                UserName = googleUser.Name,
                GoogleId = googleUser.Id
            };
            return newUser;
        }
        return null;
    }

    private async Task<GoogleJsonWebSignature.Payload> GetGoogleUserByCredential(string credential)
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

    private async Task<Google.Apis.Oauth2.v2.Data.Userinfo> GetGoogleUserByAccessToken(string accessToken)
    {
        var credential = GoogleCredential.FromAccessToken(accessToken);

        var oauth2Service = new Oauth2Service(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = Settings.GoogleApplicationName
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

    [HttpPost]
    public bool UserExists(string googleId) => _userReadingRepo.GoogleUserExists(googleId);

}