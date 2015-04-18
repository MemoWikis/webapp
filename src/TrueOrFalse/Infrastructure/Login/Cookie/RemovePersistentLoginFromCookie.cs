using System;
using System.Web;

public class RemovePersistentLoginFromCookie : IRegisterAsInstancePerLifetime
{
    private readonly GetPersistentLoginCookieValues _getPersistentLoginCookieValues;
    private readonly PersistentLoginRepository _persistentLoginRepository;

    public RemovePersistentLoginFromCookie(GetPersistentLoginCookieValues getPersistentLoginCookieValues,
                                            PersistentLoginRepository persistentLoginRepository)
    {
        _getPersistentLoginCookieValues = getPersistentLoginCookieValues;
        _persistentLoginRepository = persistentLoginRepository;
    }

    public void Run()
    {
        var persistentCookieValue = _getPersistentLoginCookieValues.Run();

        if (!persistentCookieValue.Exists())
            return;

        _persistentLoginRepository.Delete(persistentCookieValue.UserId, persistentCookieValue.LoginGuid);
        var cookie = HttpContext.Current.Response.Cookies.Get("memucho");
        cookie.Values.Set("persistentLogin", "");
        cookie.Expires = DateTime.Now.AddDays(45);
    }
}