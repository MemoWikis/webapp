public class LoginFromCookie
{
    public static bool Run()
    {
        var cookieValues = GetPersistentLoginCookieValues.Run();

        if (!cookieValues.Exists())
            return false;

        var persistentLoginRepo = Sl.R<PersistentLoginRepo>();
        var persistentLogin = persistentLoginRepo.Get(cookieValues.UserId, cookieValues.LoginGuid);

        if (persistentLogin == null)
            return false;

        var user = Sl.R<UserRepo>().GetById(cookieValues.UserId);
        if (user == null)
            return false;

        persistentLoginRepo.Delete(persistentLogin);
        WritePersistentLoginToCookie.Run(cookieValues.UserId);

        SessionUserLegacy.Login(user);            

        return true;
    }
}