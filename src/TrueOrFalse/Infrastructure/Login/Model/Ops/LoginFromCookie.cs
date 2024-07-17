using Google.Apis.Auth;
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
            RemovePersistentLoginFromCookie.RunForGoogleCredentials(httpContext);
            WritePersistentLoginToCookie.Run(sessionUser.UserId, persistentLoginRepo, httpContext);
        }
    }

    public static void RunToRestore(SessionUser sessionUser,
        PersistentLoginRepo persistentLoginRepo,
        UserReadingRepo userReadingRepo,
        string cookieString) => Run(sessionUser, persistentLoginRepo, userReadingRepo, cookieString, deletePersistentCookie: false);


    public static async Task RunToRestoreGoogle(SessionUser sessionUser, UserReadingRepo userReadingRepo, string googleCredential)
    {
        var googleUser = await GetGoogleUser(googleCredential);
        if (googleUser == null)
            throw new Exception("GoogleCredentials are invalid");

        var user = userReadingRepo.UserGetByGoogleId(googleUser.Subject);
        if (user == null)
            throw new Exception("User does not exist");

        sessionUser.Login(user);
    }
    public static async Task<GoogleJsonWebSignature.Payload?> GetGoogleUser(string token)
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