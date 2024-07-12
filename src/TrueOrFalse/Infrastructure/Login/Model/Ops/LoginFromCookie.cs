using Microsoft.AspNetCore.Http;

public class LoginFromCookie
{
    public static void Run(SessionUser sessionUser,
        PersistentLoginRepo persistentLoginRepo,
        UserReadingRepo userReadingRepo,
        string cookieString,
        HttpContext httpContext)
    {
        var cookieValues = PersistentLoginCookie.GetValues(cookieString);
        if (!cookieValues.Exists())
        {
            throw new Exception("Cookie values do not exist");
        }

        var persistentLogin = persistentLoginRepo.Get(cookieValues.UserId, cookieValues.LoginGuid);
        if (persistentLogin == null)
        {
            throw new Exception("Persistent login does not exist");
        }

        var user = userReadingRepo.GetById(cookieValues.UserId);
        if (user == null)
        {
            throw new Exception("User does not exist");
        }

        sessionUser.Login(user);

        persistentLoginRepo.Delete(persistentLogin);
        WritePersistentLoginToCookie.Run(cookieValues.UserId, persistentLoginRepo, httpContext);
    }

    public static void Run(SessionUser sessionUser,
        PersistentLoginRepo persistentLoginRepo,
        UserReadingRepo userReadingRepo,
        string cookieString)
    {
        var cookieValues = PersistentLoginCookie.GetValues(cookieString);
        if (!cookieValues.Exists())
        {
            throw new Exception("Cookie values do not exist");
        }

        var persistentLogin = persistentLoginRepo.Get(cookieValues.UserId, cookieValues.LoginGuid);
        if (persistentLogin == null)
        {
            throw new Exception("Persistent login does not exist");
        }

        var user = userReadingRepo.GetById(cookieValues.UserId);
        if (user == null)
        {
            throw new Exception("User does not exist");
        }

        sessionUser.Login(user);
    }
}