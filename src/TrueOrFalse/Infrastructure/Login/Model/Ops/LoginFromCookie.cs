public class LoginFromCookie
{
    public record struct LoginFromCookieResult(bool Success, string? LoginGuid = null, DateTimeOffset? ExpiryDate = null);

    public static LoginFromCookieResult Run(SessionUser sessionUser,
        PersistentLoginRepo persistentLoginRepo,
        UserReadingRepo userReadingRepo,
        string cookieString)
    {
        var cookieValues = PersistentLoginCookie.GetValues(cookieString);
        if (!cookieValues.Exists())
        {
            return new LoginFromCookieResult(false);
        }

        var persistentLogin = persistentLoginRepo.Get(cookieValues.UserId, cookieValues.LoginGuid);
        if (persistentLogin == null)
        {
            return new LoginFromCookieResult(false);
        }

        var user = userReadingRepo.GetById(cookieValues.UserId);
        if (user == null)
        {
            return new LoginFromCookieResult(false);
        }

        sessionUser.Login(user);

        persistentLoginRepo.Delete(persistentLogin);
        var result = WritePersistentLoginToCookie.Run(cookieValues.UserId, persistentLoginRepo);
        return new LoginFromCookieResult(true, result.LoginGuid, result.ExpiryDate);
    }
}