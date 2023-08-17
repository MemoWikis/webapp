using Microsoft.AspNetCore.Http;

public class LoginFromCookie
{
    public static bool Run(SessionUser sessionUser, 
        PersistentLoginRepo persistentLoginRepo, 
        UserReadingRepo userReadingRepo,
        IHttpContextAccessor httpContextAccessor)
    {
        var cookieValues = GetPersistentLoginCookieValues.Run(httpContextAccessor);

        if (!cookieValues.Exists())
            return false;

        var persistentLogin = persistentLoginRepo.Get(cookieValues.UserId, cookieValues.LoginGuid);

        if (persistentLogin == null)
            return false;

        var user = userReadingRepo.GetById(cookieValues.UserId);
        if (user == null)
            return false;

        persistentLoginRepo.Delete(persistentLogin);
        WritePersistentLoginToCookie.Run(cookieValues.UserId,
            persistentLoginRepo, 
            httpContextAccessor);

        sessionUser.Login(user);            

        return true;
    }
}