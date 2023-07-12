public class LoginFromCookie
{
    public static bool Run(SessionUser sessionUser, PersistentLoginRepo persistentLoginRepo, UserRepo userRepo)
    {
        var cookieValues = GetPersistentLoginCookieValues.Run();

        if (!cookieValues.Exists())
            return false;

        var persistentLogin = persistentLoginRepo.Get(cookieValues.UserId, cookieValues.LoginGuid);

        if (persistentLogin == null)
            return false;

        var user = userRepo.GetById(cookieValues.UserId);
        if (user == null)
            return false;

        persistentLoginRepo.Delete(persistentLogin);
        WritePersistentLoginToCookie.Run(cookieValues.UserId, persistentLoginRepo);

        sessionUser.Login(user);            

        return true;
    }
}