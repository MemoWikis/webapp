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
            WritePersistentLoginToCookie.Run(sessionUser.UserId, persistentLoginRepo, httpContext);
        }
    }

    public static void RunToRestore(SessionUser sessionUser,
        PersistentLoginRepo persistentLoginRepo,
        UserReadingRepo userReadingRepo,
        string cookieString) => Run(sessionUser, persistentLoginRepo, userReadingRepo, cookieString, deletePersistentCookie: false);
}