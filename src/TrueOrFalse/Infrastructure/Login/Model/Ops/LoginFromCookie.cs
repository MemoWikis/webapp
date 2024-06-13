using Microsoft.AspNetCore.Http;

public class LoginFromCookie
{
    public static bool Run(SessionUser sessionUser,
        PersistentLoginRepo persistentLoginRepo,
        UserReadingRepo userReadingRepo,
        HttpContext httpContext)
    {
        var cookieValues = PersistentLoginCookie.GetValues(httpContext);

        if (!cookieValues.Exists())
            return false;

        var persistentLogin = persistentLoginRepo.Get(cookieValues.UserId, cookieValues.LoginGuid);

        if (persistentLogin == null)
            return false;

        var user = userReadingRepo.GetById(cookieValues.UserId);
        if (user == null)
            return false;

        sessionUser.Login(user);
        persistentLoginRepo.Delete(persistentLogin);

        WritePersistentLoginToCookie.Run(cookieValues.UserId,
            persistentLoginRepo,
            httpContext);


        return true;
    }
}