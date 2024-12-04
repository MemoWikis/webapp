using Microsoft.AspNetCore.Http;
using TrueOrFalse.Domain.User;

public class LoginFromCookie
{
    private static void Run(SessionUser sessionUser,
        PersistentLoginRepo persistentLoginRepo,
        UserReadingRepo userReadingRepo,
        PageViewRepo pageViewRepo,
        string cookieString,
        bool? deletePersistentCookie = false)
    {
        var cookieValues = PersistentLoginCookie.GetValues(cookieString);
        if (!cookieValues.Exists())
        {
            var ex = new Exception("Cookie values do not exist");
            ex.Data["InvalidCookie"] = true;
            throw ex;
        }

        var persistentLogin = persistentLoginRepo.Get(cookieValues.UserId, cookieValues.LoginGuid);
        if (persistentLogin == null)
        {
            var ex = new Exception("Persistent login does not exist");
            ex.Data["InvalidCookie"] = true;
            throw ex;
        }

        var user = userReadingRepo.GetById(cookieValues.UserId);
        if (user == null)
        {
            var ex = new Exception("User does not exist");
            ex.Data["InvalidCookie"] = true;
            throw ex;
        }

        sessionUser.Login(user, pageViewRepo);

        if (deletePersistentCookie == true)
            persistentLoginRepo.Delete(persistentLogin);
    }

    public static void Run(SessionUser sessionUser,
        PersistentLoginRepo persistentLoginRepo,
        UserReadingRepo userReadingRepo,
        HttpContext httpContext,
        PageViewRepo pageViewRepo)
    {
        var cookieString = httpContext?.Request.Cookies[PersistentLoginCookie.Key];

        if (cookieString != null && httpContext != null)
        {
            Run(sessionUser, persistentLoginRepo, userReadingRepo, pageViewRepo, cookieString, deletePersistentCookie: true);
            RemovePersistentLoginFromCookie.RunForGoogle(httpContext);
            WritePersistentLoginToCookie.Run(sessionUser.UserId, persistentLoginRepo, httpContext);
        }
    }

    public static void RunToRestore(SessionUser sessionUser,
        PersistentLoginRepo persistentLoginRepo,
        UserReadingRepo userReadingRepo,
        PageViewRepo pageViewRepo,
        string cookieString) => Run(sessionUser, persistentLoginRepo, userReadingRepo, pageViewRepo, cookieString, deletePersistentCookie: false);

    public static void GoogleLogin(UserReadingRepo userReadingRepo, PageViewRepo pageViewRepo, string googleId, SessionUser sessionUser)
    {
        var user = userReadingRepo.UserGetByGoogleId(googleId);
        if (user == null)
            throw new Exception("User does not exist");

        sessionUser.Login(user, pageViewRepo);
    }

    public static async Task RunToRestoreGoogleCredential(SessionUser sessionUser, UserReadingRepo userReadingRepo, PageViewRepo pageViewRepo, GoogleLogin googleLogin, string googleCredential)
    {
        var googleUser = await googleLogin.GetGoogleUserByCredential(googleCredential);
        if (googleUser == null)
            throw new Exception("GoogleCredentials are invalid");

        GoogleLogin(userReadingRepo, pageViewRepo, googleUser.Subject, sessionUser);
    }

    public static async Task RunToRestoreGoogleAccessToken(SessionUser sessionUser, UserReadingRepo userReadingRepo, PageViewRepo pageViewRepo, GoogleLogin googleLogin, string googleAccessToken)
    {
        var googleUser = await googleLogin.GetGoogleUserByAccessToken(googleAccessToken);
        if (googleUser == null)
            throw new Exception("GoogleCredentials are invalid");

        GoogleLogin(userReadingRepo, pageViewRepo, googleUser.Id, sessionUser);
    }
}