using Microsoft.AspNetCore.Http;

public class LoginFromCookie
{
    public static void Run(SessionUser sessionUser,
        PersistentLoginRepo persistentLoginRepo,
        UserReadingRepo userReadingRepo,
        HttpContext httpContext)
    {
        var cookieValues = PersistentLoginCookie.GetValues(httpContext);

        if (!cookieValues.Exists())
            return;

        var persistentLogin = persistentLoginRepo.Get(cookieValues.UserId, cookieValues.LoginGuid);

        if (persistentLogin == null)
            return;

        var user = userReadingRepo.GetById(cookieValues.UserId);
        if (user == null)
            return;

        sessionUser.Login(user);
        persistentLoginRepo.Delete(persistentLogin);

        WritePersistentLoginToCookie.Run(cookieValues.UserId, persistentLoginRepo, httpContext);
    }
}