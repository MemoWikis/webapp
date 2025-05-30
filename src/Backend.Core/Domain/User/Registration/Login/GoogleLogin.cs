﻿using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Oauth2.v2;
using Google.Apis.Services;
using Microsoft.AspNetCore.Http;

public class GoogleLogin(
    SessionUser _sessionUser,
    IHttpContextAccessor _httpContextAccessor,
    RegisterUser _registerUser,
    PersistentLoginRepo _persistentLoginRepo,
    UserReadingRepo _userReadingRepo,
    UserWritingRepo _userWritingRepo,
    PageViewRepo _pageViewRepo) : IRegisterAsInstancePerLifetime
{
    public async Task<(bool Success, string? MessageKey)> Login(string language, string? credential = null, string? accessToken = null)
    {
        var googleUser = await GetGoogleUser(credential, accessToken);
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

                var result = _registerUser.CreateAndLogin(newUser, language);
                AppendGoogleCredentialCookie(_httpContextAccessor.HttpContext, credential, accessToken);

                return (true, result.MessageKey);
            }

            _sessionUser.Login(user, _pageViewRepo);
            _userWritingRepo.Update(user);
            AppendGoogleCredentialCookie(_httpContextAccessor.HttpContext, credential, accessToken);


            return (true, null);
        }

        return (false, null);
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
            Log.Error(e.ToString());
            return null;
        }
    }

    public async Task<Google.Apis.Oauth2.v2.Data.Userinfo> GetGoogleUserByAccessToken(string accessToken)
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
            Log.Error(e.ToString());
            return null;
        }
    }
}