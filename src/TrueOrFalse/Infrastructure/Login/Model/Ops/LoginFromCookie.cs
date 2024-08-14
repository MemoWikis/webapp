using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Oauth2.v2;
using Google.Apis.Services;
using Microsoft.AspNetCore.Http;

public class LoginFromCookie
{
    private static void Run(SessionUser sessionUser,
        PersistentLoginRepo persistentLoginRepo,
        UserReadingRepo userReadingRepo,
        string cookieString,
        bool? deletePersistentCookie = false)
    {
        var cookieValues = PersistentLoginCookie.GetValues(cookieString);
        if (!cookieValues.Exists())
            throw new Exception("Cookie values do not exist");

        var persistentLogin = persistentLoginRepo.Get(cookieValues.UserId, cookieValues.LoginGuid);
        if (persistentLogin == null)
            throw new Exception("Persistent login does not exist");

        var user = userReadingRepo.GetById(cookieValues.UserId);
        if (user == null)
            throw new Exception("User does not exist");

        sessionUser.Login(user);

        if (deletePersistentCookie == true)
            persistentLoginRepo.Delete(persistentLogin);
    }

    public static void Run(SessionUser sessionUser,
        PersistentLoginRepo persistentLoginRepo,
        UserReadingRepo userReadingRepo,
        HttpContext httpContext)
    {
        var cookieString = httpContext?.Request.Cookies[PersistentLoginCookie.Key];

        if (cookieString != null && httpContext != null)
        {
            Run(sessionUser, persistentLoginRepo, userReadingRepo, cookieString, deletePersistentCookie: true);
            RemovePersistentLoginFromCookie.RunForGoogle(httpContext);
            WritePersistentLoginToCookie.Run(sessionUser.UserId, persistentLoginRepo, httpContext);
        }
    }

    public static void GoogleLogin(UserReadingRepo userReadingRepo, string googleId, SessionUser sessionUser)
    {
        var user = userReadingRepo.UserGetByGoogleId(googleId);
        if (user == null)
            throw new Exception("User does not exist");

        sessionUser.Login(user);
    }

    public static void RunToRestore(SessionUser sessionUser,
        PersistentLoginRepo persistentLoginRepo,
        UserReadingRepo userReadingRepo,
        string cookieString) => Run(sessionUser, persistentLoginRepo, userReadingRepo, cookieString, deletePersistentCookie: false);

    private static async Task<GoogleJsonWebSignature.Payload?> GetGoogleUserByCredential(string token)
    {
        var settings = new GoogleJsonWebSignature.ValidationSettings
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

    public static async Task RunToRestoreGoogleCredential(SessionUser sessionUser, UserReadingRepo userReadingRepo, string googleCredential)
    {
        var googleUser = await GetGoogleUserByCredential(googleCredential);
        if (googleUser == null)
            throw new Exception("GoogleCredentials are invalid");

        GoogleLogin(userReadingRepo, googleUser.Subject, sessionUser);
    }

    private static async Task<Google.Apis.Oauth2.v2.Data.Userinfo> GetGoogleUserByAccessToken(string accessToken)
    {
        var credential = GoogleCredential.FromAccessToken(accessToken);

        var oauth2Service = new Oauth2Service(new BaseClientService.Initializer
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

    public static async Task RunToRestoreGoogleAccessToken(SessionUser sessionUser, UserReadingRepo userReadingRepo, string googleAccessToken)
    {
        var googleUser = await GetGoogleUserByAccessToken(googleAccessToken);
        if (googleUser == null)
            throw new Exception("GoogleCredentials are invalid");

        GoogleLogin(userReadingRepo, googleUser.Id, sessionUser);
    }
}